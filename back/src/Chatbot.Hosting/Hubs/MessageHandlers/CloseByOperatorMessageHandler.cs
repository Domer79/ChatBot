using System;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Pipe;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class CloseByOperatorMessageHandler: PipeHandler<IMessagePipeContext>
    {
        private readonly IHubContext<ChatHub> _chatHubContext;

        public CloseByOperatorMessageHandler(IHubContext<ChatHub> chatHubContext)
        {
            _chatHubContext = chatHubContext;
        }

        protected override Task InvokeAsync(IMessagePipeContext context, Func<IPipeContext, Task> next)
        {
            if (context.Message.Type == MessageType.CloseSession && context.Message.Owner == MessageOwner.Operator)
            {
                var dialogGroup = context.DialogGroup;
                _chatHubContext.Clients.Client(dialogGroup.ClientConnection.ConnectionId)
                    .SendAsync("send", context.Message);
            }

            return next(context);
        }
    }
}