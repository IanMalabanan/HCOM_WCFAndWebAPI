using Hcom.Web.Api.Core;
using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Repositories;
using Hcom.Web.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSvcRefence;

namespace Hcom.Web.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDistributedMemoryCache();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IUserService, UserServiceClient>();
            services.AddScoped<IFrebasChangeForgotPasswordAPIService, FrebasChangeForgotPasswordAPIService>();
            services.AddScoped<IMilestone, MilestoneService>();
            services.AddScoped<IUnit, UnitService>();
            services.AddScoped<INotification, NotificationService>();
            services.AddScoped<IPunchlist, PunchListService>();
            services.AddScoped<IUser, UserService>();
            services.AddScoped<ICacheProvider, CacheProviderService>();
            services.AddScoped<UserInfoProvider>();
            services.AddScoped<IFileUploadRepository, FileUploadRepository>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<ServiceEndpoints>(Configuration.GetSection("WCFEndpointAddress"));
            services.Configure<FTPSettings>(Configuration.GetSection("FTPSettings"));
            services.Configure<ConfigSettings>(Configuration.GetSection("ConfigSettings"));

            return services;
        }
    }
}
