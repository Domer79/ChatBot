using Autofac;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Hosting.Hubs;
using Chatbot.Hosting.Misc;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting
{
    public class HubModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserSet>().AsSelf().SingleInstance();
            builder.RegisterType<DialogActiveCollection>().AsSelf().SingleInstance();
            builder.RegisterType<HubDispatcher>().As<IHubDispatcher>().SingleInstance();
            builder.RegisterType<Misc.Logger<ChatHub>>().As<ILogger<ChatHub>>();
        }
    }

    public class HostingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().AsSelf();
        }
    }
}