using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Chatbot.Hosting
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var env = "development";
            if (args.All(_ => _ != "--environment"))
            {
                var argsList = new List<string>(args);
                argsList.Add("--environment");
                argsList.Add("development");
                args = argsList.ToArray();
            }
            else
            {
                var index = -1;
                do
                {
                    index++;
                } while (args[index] != "--environment");

                env = args[index + 1];
            }
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables();
            var config = builder.Build();

            var host = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseConfiguration(config)
                        .UseUrls("http://localhost:5011")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseWebRoot("wwwroot")
                        .UseStartup<Startup>();
                })
                .Build();

            host.Run();
        }
    }
}