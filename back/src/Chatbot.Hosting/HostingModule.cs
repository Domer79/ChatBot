using Autofac;
using Chatbot.Abstractions;
using Chatbot.Hosting.Hubs;
using Chatbot.Hosting.Misc;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting
{
    public class HostingModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HubDispatcher>().As<IHubDispatcher>().SingleInstance();
            builder.RegisterType<Mapper>().AsSelf().SingleInstance();
            builder.RegisterType<Misc.Logger<ChatHub>>().As<ILogger<ChatHub>>();
        }
    }
}