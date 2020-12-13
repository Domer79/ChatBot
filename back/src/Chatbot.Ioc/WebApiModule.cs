using System;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Chatbot.Common;
using Chatbot.Common.Abstracts;

namespace Chatbot.Ioc
{
    public class WebApiModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppConfig>().As<IAppConfig>();
        }
    }
}