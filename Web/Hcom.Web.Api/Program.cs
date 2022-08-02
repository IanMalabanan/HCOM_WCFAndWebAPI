using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Hcom.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
         var  host = CreateWebHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Logging Initiated");

            host.Run();
        }




        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
          return  WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.ClearProviders();
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                     logging.AddDebug();
                     logging.AddSerilog();
                     logging.AddEventSourceLogger();
                 });

            //var builtConfig = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddCommandLine(args)
            //    .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.File(builtConfig["Logging:FilePath"])
            //    .CreateLogger();

            //try
            //{
            //    return WebHost.CreateDefaultBuilder(args)
            //        .ConfigureLogging((hostingContext, logging) =>
            //        {
            //            // Requires `using Microsoft.Extensions.Logging;`
            //            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //            logging.AddConsole();
            //            logging.AddDebug();
            //            logging.AddEventSourceLogger();
            //        })
            //        .UseStartup<Startup>();
            //}
            //catch (Exception ex)
            //{
            //    Log.Fatal(ex, "Host builder error");

            //    throw;
            //}
            //finally
            //{
            //    Log.CloseAndFlush();
            //}
        }
    }
}
