using System;
using System.Threading;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Pipe;
using Chatbot.Common;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class CreateDialogHandler: PipeHandler<IMessagePipeContext>
    {
        private readonly IHubContext<OperatorHub> _operatorHub;
        private readonly IHubDispatcher _hubDispatcher;
        private readonly IChatBotHelper _chatBotHelper;

        public CreateDialogHandler(
            IHubContext<OperatorHub> operatorHub,
            IHubDispatcher hubDispatcher,
            IChatBotHelper chatBotHelper)
        {
            _operatorHub = operatorHub;
            _hubDispatcher = hubDispatcher;
            _chatBotHelper = chatBotHelper;
        }

        protected override async Task InvokeAsync(IMessagePipeContext context, Func<IPipeContext, Task> next)
        {
            var message = context.Message;
            var chatHub = context.ChatHub;
            var dialogGroup = await _hubDispatcher.GetOrCreateDialogGroup(context.User, message.MessageDialogId);
            if (message.MessageDialogId.IsEmpty())
            {
                message.MessageDialogId = dialogGroup.MessageDialogId;
                dialogGroup.ClientId = context.User.Id;
                await chatHub.Clients.Caller.SendAsync("send", await _chatBotHelper.GetResponse(message));
                Thread.Sleep(10);
                await chatHub.Clients.Caller.SendAsync("sendQuestions", await _chatBotHelper.GetQuestionMessages(message));
                Thread.Sleep(10);
                await chatHub.Clients.Caller.SendAsync("sendButton", await _chatBotHelper.GetButtonForForm(message));
            }

            if (context.IsNewDialog)
            {
                await _operatorHub.Clients.All.SendAsync("dialogCreated", dialogGroup.MessageDialogId);
            }

            context.DialogGroup = dialogGroup;
            await next(context);
        }
    }
}