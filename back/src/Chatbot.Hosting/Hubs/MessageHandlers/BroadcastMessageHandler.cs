using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Pipe;
using Chatbot.Core.Common;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class BroadcastMessageHandler: PipeHandler<IMessagePipeContext>
    {
        private readonly Mapper _mapper;

        public BroadcastMessageHandler(Mapper mapper)
        {
            _mapper = mapper;
        }

        protected override async Task InvokeAsync(IMessagePipeContext context, Func<IPipeContext, Task> next)
        {
            if (context.DialogGroup.MemberCount > 1)
            {
                await context.ChatHub.Clients.Clients(context.DialogGroup.Others(context.User.Id))
                    .SendAsync("send", _mapper.Map<MessageResponse>(context.Message));
            }

            await next(context);
        }
    }
}