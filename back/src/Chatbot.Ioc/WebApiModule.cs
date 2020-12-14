using System;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Core;
using Chatbot.Core.Common;
using Chatbot.Core.Services;
using Chatbot.Ef.Data;

namespace Chatbot.Ioc
{
    public class WebApiModule: Module
    {
        private ContainerBuilder _containerBuilder;

        protected override void Load(ContainerBuilder builder)
        {
            _containerBuilder = builder;
            RegisterCommon();
            RegisterRepository();
            RegisterServices();
        }

        private void RegisterRepository()
        {
            _containerBuilder.RegisterType<UserRepository>().As<IUserRepository>();
            _containerBuilder.RegisterType<TokenRepository>().As<ITokenRepository>();
            _containerBuilder.RegisterType<RoleRepository>().As<IRoleRepository>();
        }

        private void RegisterServices()
        {
            _containerBuilder.RegisterType<UserService>().As<IUserService>();
            _containerBuilder.RegisterType<TokenService>().As<ITokenService>();
            _containerBuilder.RegisterType<RoleService>().As<IRoleService>();
            _containerBuilder.RegisterType<AuthService>().As<IAuthService>();
        }

        private void RegisterCommon()
        {
            _containerBuilder.RegisterType<AppConfig>().As<IAppConfig>();
        }
    }
}