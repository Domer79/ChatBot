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
            var config = StartupHelper.GetConfiguration(args);

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