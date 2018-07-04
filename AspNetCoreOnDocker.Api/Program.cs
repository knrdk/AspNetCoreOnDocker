using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace AspNetCoreOnDocker.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            string internalLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "aspnetcoreondocker-nlog-internals.txt");
            // this is workaround for https://github.com/NLog/NLog.Web/issues/201
            NLog.Common.InternalLogger.LogFile = internalLogPath;
            NLog.Web.NLogBuilder.ConfigureNLog("nlog.config");           
            
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
    }
}
