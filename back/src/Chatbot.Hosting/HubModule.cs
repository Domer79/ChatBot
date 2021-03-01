using Autofac;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Pipe;
using Chatbot.Core.Chat;
using Chatbot.Core.Common;
using Chatbot.Hosting.Hubs;
using Chatbot.Hosting.Hubs.MessageHandlers;
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
            builder.RegisterType<CreateDialogHandler>().As<PipeHandler<IMessagePipeContext>>().AsSelf();
            builder.RegisterType<ClosedMessageHandler>().As<PipeHandler<IMessagePipeContext>>().AsSelf();
            builder.RegisterType<BroadcastMessageHandler>().As<PipeHandler<IMessagePipeContext>>().AsSelf();
            builder.RegisterType<SaveMessageHandler>().As<PipeHandler<IMessagePipeContext>>().AsSelf();
            builder.RegisterType<ValidateDialogHandler>().As<PipeHandler<IMessagePipeContext>>().AsSelf();
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