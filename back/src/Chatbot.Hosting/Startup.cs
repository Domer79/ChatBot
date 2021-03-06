﻿using System;
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
using Chatbot.Hosting.Extensions;
using Chatbot.Hosting.Hubs;
using Chatbot.Hosting.Hubs.MessageHandlers;
using Chatbot.Hosting.Hubs.Pipes;
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
        private ISettingsService _settingsService;

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
            }, ServiceLifetime.Transient);
            services.AddTransient<IAuthenticationHandler, TokenAuthenticationHandler>();
            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, CustomAuthPolicyProvider>();
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
                    var clientTimeoutInterval = _settingsService.GetClientTimeoutInterval().GetAwaiter().GetResult() ?? 20;
                    options.EnableDetailedErrors = true;
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(clientTimeoutInterval);
                    options.KeepAliveInterval = TimeSpan.FromMinutes(clientTimeoutInterval / 2.0);
                    options.HandshakeTimeout = TimeSpan.FromSeconds(60);
                })
                .AddHubOptions<OperatorHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(20);
                    options.KeepAliveInterval = TimeSpan.FromSeconds(60);
                    options.HandshakeTimeout = TimeSpan.FromSeconds(60);
                })
                .AddHubOptions<TokenHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(20);
                    options.KeepAliveInterval = TimeSpan.FromSeconds(60);
                    options.HandshakeTimeout = TimeSpan.FromSeconds(60);
                });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });
            
            services.AddCors();
            services.AddPipe<IMessagePipe, MessagePipe>(options =>
            {
                options.AddHandlerService<CreateDialogHandler>();
                options.AddHandlerService<ValidateDialogHandler>();
                options.AddHandlerService<SaveMessageHandler>();
                options.AddHandlerService<ClosedMessageHandler>();
                options.AddHandlerService<BroadcastMessageHandler>();
                return options;
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<WebApiModule>();
            builder.RegisterModule<HostingModule>();
            builder.RegisterModule<HubModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (!env.EnvironmentName.EndsWith("-hub"))
            {
                var context = AutofacContainer.Resolve<ChatbotContext>();
                var permissionService = AutofacContainer.Resolve<IPermissionService>();
            
                context.Database.Migrate();
                permissionService.RefreshPolicy().GetAwaiter().GetResult();
            }
            _settingsService = AutofacContainer.Resolve<ISettingsService>();
            _settingsService.SetDefaultSettings().GetAwaiter().GetResult();
            
            var logger = loggerFactory.CreateLogger<Startup>();
            app.Use(async (context, next) => await ErrorHandle(context, next, logger));
            app.Use(async (context, next) => await AuthQueryStringToHeader(context, next));
            app.UseCors(builder =>
            {
                var origins = _configuration.GetSection("HttpServer:Endpoints").GetChildren().Select(_ =>
                {
                    var endpoint = new EndpointConfiguration();
                    _.Bind(endpoint);
                    return endpoint.CorsOrigins;
                }).ToArray();
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(origins)
                    .SetIsOriginAllowed(h => true);
            });
            if (env.IsDevelopment() || env.EnvironmentName == "dev-hub")
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

                if (env.EnvironmentName.EndsWith("-hub"))
                {
                    endpoints.MapHub<ChatHub>("/chat");
                    endpoints.MapHub<OperatorHub>("/dialog");
                    endpoints.MapHub<TokenHub>("/token").AllowAnonymous();
                }
            });
        }

        private async Task ErrorHandle(HttpContext context, Func<Task> next, ILogger<Startup> logger)
        {
            try
            {
                await next.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError(e, e.Message);
                await next.Invoke();
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