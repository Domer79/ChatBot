using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Pipe;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class SaveMessageHandler: PipeHandler<IMessagePipeContext>
    {
        private readonly IMessageService _messageService;

        public SaveMessageHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        protected override async Task InvokeAsync(IMessagePipeContext context, Func<IPipeContext, Task> next)
        {
            context.Message.Status = MessageStatus.Saved;
            context.Message.MessageDialogId = context.DialogGroup.MessageDialogId;
            
            context.Message = await _messageService.Add(context.Message);
            context.DialogGroup.LastMessageTime = context.Message.Time;
            await context.ChatHub.Clients.Caller.SendAsync("setMeta", context.Message);

            await next(context);
        }
    }
}