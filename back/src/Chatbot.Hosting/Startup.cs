using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using Newtonsoft.Json;

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
                    .AddAuthenticationSchemes(TokenAuthenticationOptions.SchemeName)
                    .Build();
            });
            services.AddSignalR()
                .AddHubOptions<ChatHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
                })
                .AddHubOptions<OperatorHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(20);
                });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            
            services.AddCors();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<WebApiModule>();
            builder.RegisterModule<HostingModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            var context = AutofacContainer.Resolve<ChatbotContext>();
            var permissionService = AutofacContainer.Resolve<IPermissionService>();
            
            context.Database.Migrate();
            permissionService.RefreshPolicy();
            
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200", "http://localhost:4201")
                    .SetIsOriginAllowed(h => true);
            });
            app.Use(async (context, next) => await AuthQueryStringToHeader(context, next));
            app.Use(async (context, next) => await ErrorHandle(context, next));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<OperatorHub>("/dialog");
                endpoints.MapHub<TokenHub>("/token").AllowAnonymous();
            });
        }

        private async Task ErrorHandle(HttpContext context, Func<Task> next)
        {
            try
            {
                await next.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task AuthQueryStringToHeader(HttpContext context, Func<Task> next)
        {
            var qs = context.Request.QueryString;

            if (string.IsNullOrWhiteSpace(context.Request.Headers["token"]) && qs.HasValue)
            {
                var token = (from pair in qs.Value.TrimStart('?').Split('&')
                    where pair.StartsWith("token=")
                    select pair.Substring(6)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    context.Request.Headers.Add("token", token);
                }
            }

            await next.Invoke();
        }
    }
}