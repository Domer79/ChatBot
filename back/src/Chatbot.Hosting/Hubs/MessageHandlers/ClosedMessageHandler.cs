using System;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Pipe;
using Chatbot.Model.Enums;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class ClosedMessageHandler: PipeHandler<IMessagePipeContext>
    {
        private readonly IHubDispatcher _hubDispatcher;

        public ClosedMessageHandler(IHubDispatcher hubDispatcher)
        {
            _hubDispatcher = hubDispatcher;
        }

        protected override async Task InvokeAsync(IMessagePipeContext context, Func<IPipeContext, Task> next)
        {
            if (context.Message.Type == MessageType.CloseSession)
            {
                await _hubDispatcher.CloseClientDialog(context.User.Id);
            }
            
            await next(context);
        }
    }
}