using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Chatbot.Hosting.Misc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Chatbot.Hosting
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                var config = StartupHelper.GetConfiguration(args);
                var host = Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        // logging.ClearProviders();
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.SetMinimumLevel(LogLevel.Trace);
                        // logging.AddDebug();
                        // logging.AddNLog();
                        // logging.AddProvider(new MyLoggerProvider());
                    })
                    .UseNLog()
                    .ConfigureWebHostDefaults(builder =>
                    {
                        builder.UseConfiguration(config)
                            .UseUrls(config.GetValue<string>("BaseUrl"))
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseWebRoot("wwwroot")
                            .UseStartup<Startup>();
                    })
                    .Build();

                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped program because of exception");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}