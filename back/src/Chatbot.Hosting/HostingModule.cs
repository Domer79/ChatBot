using Autofac;
using Chatbot.Abstractions;
using Chatbot.Hosting.Hubs;
using Chatbot.Hosting.Misc;

namespace Chatbot.Hosting
{
    public class HostingModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HubDispatcher>().As<IHubDispatcher>().SingleInstance();
            builder.RegisterType<Mapper>().AsSelf().SingleInstance();
        }
    }
}