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
using Chatbot.Model.DataModel;

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
            _containerBuilder.RegisterType<MessageDialogRepository>().As<IMessageDialogRepository>();
            _containerBuilder.RegisterType<MessageRepository>().As<IMessageRepository>();
            _containerBuilder.RegisterType<QuestionRepository>().As<IQuestionRepository>();
            _containerBuilder.RegisterType<RolePermissionRepository>().As<IRolePermissionRepository>();
            _containerBuilder.RegisterType<PermissionRepository>().As<IPermissionRepository>();
            _containerBuilder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>();
        }

        private void RegisterServices()
        {
            _containerBuilder.RegisterType<UserService>().As<IUserService>();
            _containerBuilder.RegisterType<TokenService>().As<ITokenService>();
            _containerBuilder.RegisterType<RoleService>().As<IRoleService>();
            _containerBuilder.RegisterType<AuthService>().As<IAuthService>();
            _containerBuilder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            _containerBuilder.RegisterType<MessageService>().As<IMessageService>();
            _containerBuilder.RegisterType<PermissionService>().As<IPermissionService>();
            _containerBuilder.RegisterType<QuestionService>().As<IQuestionService>();
        }

        private void RegisterCommon()
        {
            _containerBuilder.RegisterType<AppConfig>().As<IAppConfig>();
        }
    }
}