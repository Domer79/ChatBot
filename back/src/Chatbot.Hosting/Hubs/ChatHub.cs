using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
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
        private readonly ITokenService _tokenService;
        private readonly IHubDispatcher _hubDispatcher;
        private readonly IHubContext<OperatorHub> _operatorHubContext;
        private readonly IOperatorLogService _logService;
        private readonly Mapper _mapper;

        public ChatHub(IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService,
            ITokenService tokenService,
            IHubDispatcher hubDispatcher,
            IHubContext<OperatorHub> operatorHubContext,
            IOperatorLogService logService,
            Mapper mapper)
        : base(userService)
        {
            _messageService = messageService;
            _messageDialogService = messageDialogService;
            _userService = userService;
            _tokenService = tokenService;
            _hubDispatcher = hubDispatcher;
            _operatorHubContext = operatorHubContext;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task Send(Message message)
        {
            var user = await GetUser();
            message.Sender = user.Id;

            DialogGroup dialogGroup;
            if (message.MessageDialogId.IsEmpty())
            {
                dialogGroup = await _hubDispatcher.CreateGroup(user, Context.ConnectionId);
                message.MessageDialogId = dialogGroup.MessageDialogId;
                await Groups.AddToGroupAsync(Context.ConnectionId, dialogGroup.Name);
                await _operatorHubContext.Clients.All.SendAsync("dialogCreated", dialogGroup.MessageDialogId);
            }
            
            dialogGroup = _hubDispatcher.GetDialogGroup(message.MessageDialogId);
            if (dialogGroup == null)
                throw new InvalidOperationException($"Group by message dialog id '{message.MessageDialogId}' not found");
            if (!dialogGroup.UserExist(user))
            {
                throw new InvalidOperationException($"User {user.Login} not connected in chat room");
            }

            message.Status = MessageStatus.Saved;
            message = await _messageService.Add(message);
            dialogGroup.LastMessageTime = message.Time;

            await Clients.Caller.SendAsync("meta", _mapper.Map<MessageResponse>(message));

            if (dialogGroup.MemberCount > 1)
            {
                await Clients.OthersInGroup(dialogGroup.Name).SendAsync("send", _mapper.Map<MessageResponse>(message));
            }
        }

        [CustomSecurity(SecurityPolicy.OperatorConnect)]
        public async Task OperatorConnect(Guid messageDialogId)
        {
            var user = await GetUser();
            var dialogGroup = _hubDispatcher.GetDialogGroup(messageDialogId);
            dialogGroup.AddUser(user, Context.ConnectionId, true);
            await _logService.Log(user.Id, $"Operator {user.Login} connect to dialog {messageDialogId}");
            await Clients.Caller.SendAsync("OperatorConnect", "success");
        }

        public override async Task OnConnectedAsync()
        {
            await Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} open");
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

            await CheckDeprecated();

            if (isOperatorConnection)
                await _logService.Log(user.Id,
                    $"Operator disconnect from {string.Join(',', dialogGroups.Select(_ => _.MessageDialogId))}");
        }

        private async Task CheckDeprecated()
        {
            var dialogGroups = _hubDispatcher.GetDialogGroups().Where(_ => _.IsDeprecated).ToArray();
            foreach (var dialogGroup in dialogGroups)
            {
                await Clients.Clients(dialogGroup.GetAllConnectionIds())
                    .SendAsync("closeDialog", dialogGroup.MessageDialogId);

                await _hubDispatcher.RemoveDialogGroup(dialogGroup);
            }
        }
    }
}