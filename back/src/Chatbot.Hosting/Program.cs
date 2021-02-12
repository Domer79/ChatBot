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
            var nlogOptions = new NLogAspNetCoreOptions()
            {
                IncludeScopes = true,
            };
            try
            {
                logger.Debug("init main");
                var config = StartupHelper.GetConfiguration(args);
                var host = Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                    .UseNLog(nlogOptions)
                    .ConfigureWebHostDefaults(builder =>
                    {
                        builder.UseConfiguration(config)
                            .UseKestrel(options => options.ConfigureEndpoints())
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
                Console.WriteLine(e);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}