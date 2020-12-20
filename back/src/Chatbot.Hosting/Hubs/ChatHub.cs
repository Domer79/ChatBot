using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public class ChatHub: Hub
    {
        
    }

    public class DialogHub : Hub
    {
        private readonly IHubDispatcher _hubDispatcher;

        public DialogHub(IHubDispatcher hubDispatcher)
        {
            _hubDispatcher = hubDispatcher;
        }

        public async Task TakeToWork()
        {
            var user = Context.User;
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

    public class HubDispatcher : IHubDispatcher
    {
        private readonly DialogHub _dialogHub;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMessageService _messageService;
        private readonly List<string> _activeOperators = new();

        public HubDispatcher(DialogHub dialogHub, 
            IMessageDialogService messageDialogService,
            IMessageService messageService
            )
        {
            _dialogHub = dialogHub;
            _messageDialogService = messageDialogService;
            _messageService = messageService;
        }

        public async Task<Message> DialogCreate(Message message)
        {
            var messageDialog = new MessageDialog()
            {
                DialogStatus = DialogStatus.Started,
            };

            messageDialog = await _messageDialogService.Start(messageDialog);
            message.MessageDialogId = messageDialog.Id;
            await _messageService.Add(message);
            
            await _dialogHub.Clients.Users(_activeOperators).SendAsync("create", messageDialog);
            return message;
        }

        public Task OperatorConnect(string userIdentifier)
        {
            _activeOperators.Add(userIdentifier);
            return Task.CompletedTask;
        }

        public Task OperatorDisconnect(string? contextUserIdentifier)
        {
            throw new NotImplementedException();
        }
    }
}