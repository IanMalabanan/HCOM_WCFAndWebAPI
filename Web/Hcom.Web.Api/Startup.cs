using System;
using AutoMapper;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Hcom.Web.Api.Utilities.Security;
using Hcom.Web.Api.Services;
using System.Linq;

namespace Hcom.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("Hcom.Web.Api"));
            });

            services.AddDbContext<FileUploadDBContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:FileUploadConnection"], b => b.MigrationsAssembly("Hcom.Web.Api"));
            });

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<DbContext, FileUploadDBContext>();

            // add identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;


                //options.SignIn.RequireConfirmedEmail = true;

                //    //// Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                //    //// Lockout settings
                //    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //    //options.Lockout.MaxFailedAccessAttempts = 10;
            });

            // Add cors
            services.AddCors();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );


            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);


            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParams);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt => {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParams;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ManageAllUsersPolicy, 
                    policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers));


            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("AUTH", new OpenApiInfo { Title = "Authorization Server", Version = "v1" });
                c.SwaggerDoc("UTILITY", new OpenApiInfo { Title = "Utilities", Version = "v1" });
                c.SwaggerDoc("HCOM", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HCOM - House Inspection UAT Env",
                    Description = "Rest APIs for House Inspection Mobile App",
                    TermsOfService = null,
                    Contact = new OpenApiContact
                    {
                        Name = "CTI",
                        Email = "appsdev@filinvestland.com",
                        Url = new Uri("https://www.filinvest.com.ph")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://www.filinvest.com.ph")
                    }
                });
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer(Token) Authentication", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.OperationFilter<FileUploadOperation>();
            });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDependency(Configuration);

            

            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // other code remove for clarity 
            loggerFactory.AddFile("Logs/mylogTest-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();
           
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();
           

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "CTI - Rest API Services";
                c.SwaggerEndpoint("/swagger/AUTH/swagger.json", "AUTH");
                c.SwaggerEndpoint("/swagger/HCOM/swagger.json", "HCOM");
                c.SwaggerEndpoint("/swagger/UTILITY/swagger.json", "UTILITY");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors();
        }
    }
}
