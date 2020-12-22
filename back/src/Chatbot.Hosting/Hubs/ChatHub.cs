using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    [Authorize]
    public class ChatHub: HubBase
    {
        private readonly IMessageService _messageService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IUserService _userService;
        private readonly IHubDispatcher _hubDispatcher;
        private readonly IHubContext<OperatorHub> _operatorHubContext;
        private readonly IOperatorLogService _logService;

        public ChatHub(IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService,
            IHubDispatcher hubDispatcher,
            IHubContext<OperatorHub> operatorHubContext,
            IOperatorLogService logService)
        : base(userService)
        {
            _messageService = messageService;
            _messageDialogService = messageDialogService;
            _userService = userService;
            _hubDispatcher = hubDispatcher;
            _operatorHubContext = operatorHubContext;
            _logService = logService;
        }

        public async Task Send(Message message)
        {
            var user = await GetUser();
            message.Sender = user.Id;

            DialogGroup dialogGroup;
            if (message.MessageDialogId.IsEmpty())
            {
                dialogGroup = await _hubDispatcher.CreateGroup(user, Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, dialogGroup.Name);
            }
            dialogGroup = _hubDispatcher.GetDialogGroup(message.MessageDialogId);
            if (dialogGroup == null)
                throw new InvalidOperationException($"Group by message dialog id '{message.MessageDialogId}' not found");

            if (!dialogGroup.UserExist(user))
            {
                throw new InvalidOperationException($"User {user.Login} not connected in chat room");
            }
            
            message.MessageDialogId = dialogGroup.MessageDialogId;
            await _messageService.Add(message);

            if (dialogGroup.MemberCount > 1)
            {
                await Clients.OthersInGroup(dialogGroup.Name).SendAsync("send", message);
            }
        }

        public async Task OperatorConnect(Guid messageDialogId)
        {
            var user = await GetUser();
            var dialogGroup = _hubDispatcher.GetDialogGroup(messageDialogId);
            dialogGroup.AddUser(await GetUser(), Context.ConnectionId, true);
            await _logService.Log(user.Id, $"Operator {user.Login} connect to dialog {messageDialogId}");
            await Clients.Caller.SendAsync("OperatorConnect", "success");
        }

        public override Task OnConnectedAsync()
        {
            return Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} open");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await GetUser();
            DialogGroup[] dialogGroups = _hubDispatcher.GetDialogGroups(Context.ConnectionId);
            bool isOperatorConnection = _hubDispatcher.CheckOperator(user);
            foreach (var dialogGroup in dialogGroups)
            {
                dialogGroup.RemoveUser(Context.ConnectionId);
            }

            if (isOperatorConnection)
                await _logService.Log(user.Id,
                    $"Operator disconnect from {string.Join(',', dialogGroups.Select(_ => _.MessageDialogId))}");
        }
    }
}