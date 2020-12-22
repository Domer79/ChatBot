using System;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public class OperatorHub : HubBase
    {
        private readonly IHubDispatcher _hubDispatcher;
        private readonly IOperatorLogService _logService;
        private readonly IMessageService _messageService;
        private readonly IMessageDialogService _messageDialogService;

        public OperatorHub(IHubDispatcher hubDispatcher,
            IOperatorLogService logService,
            IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService)
            : base(userService)
        {
            _hubDispatcher = hubDispatcher;
            _logService = logService;
            _messageService = messageService;
            _messageDialogService = messageDialogService;
        }

        public async Task<Message[]> TakeToWork(MessageDialog dialog)
        {
            var dialogOperator = await GetUser();
            await _logService.Log(dialogOperator.Id, $"Dialog {dialog.Id} taken to work");
            dialog.OperatorId = dialogOperator.Id;
            await _messageDialogService.Activate(dialog);
            return await _messageService.GetDialogMessages(dialog.Id);
        }

        public override async Task OnConnectedAsync()
        {
            await _hubDispatcher.OperatorConnect(Context.UserIdentifier);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _hubDispatcher.OperatorDisconnect(Context.UserIdentifier);
        }
    }
}