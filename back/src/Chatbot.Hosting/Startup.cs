﻿using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Extensions.DependencyInjection;
using Chatbot.Common.Abstracts;
using Chatbot.Data;
using Chatbot.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting
{
    public class Startup
    {
        private readonly IAppConfig _appConfig;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<ChatbotContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("default"));
            });
            
            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new WebApiModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            var context = AutofacContainer.Resolve<ChatbotContext>();
            context.Database.Migrate();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseMvc();
            app.UseStaticFiles();
            // app.UseAuthentication();
            // app.UseAuthorization();
            // app.UseWebSockets();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public ILifetimeScope AutofacContainer { get; set; }
    }

    public class AutofacModule : IModule
    {
        public void Configure(IComponentRegistryBuilder componentRegistry)
        {
            
        }
    }
}