using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Extensions.DependencyInjection;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Ef;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Hubs;
using Chatbot.Ioc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        private ILifetimeScope AutofacContainer { get; set; }

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
            services.AddDbContext<ChatbotContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("default"));
            });
            services.AddSingleton<IAuthenticationHandler, TokenAuthenticationHandler>();
            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthPolicyProvider>();
            services.AddAuthentication("Token")
                .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>("Token", null);
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Token")
                    .Build();
            });
            services.AddSignalR(options =>
            {
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
            var permissionService = AutofacContainer.Resolve<IPermissionService>();
            
            context.Database.Migrate();
            permissionService.RefreshPolicy();
            
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
            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseWebSockets();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<DialogHub>("/dialog");
            });
        }
    }
}