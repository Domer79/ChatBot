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
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting.Hubs
{
    [Authorize]
    public class ChatHub: HubBase
    {
        private readonly ITokenService _tokenService;
        private readonly IHubContext<OperatorHub> _operatorHubContext;
        private readonly IOperatorLogService _logService;
        private readonly ILogger<ChatHub> _logger;
        private readonly Mapper _mapper;

        public ChatHub(IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService,
            ITokenService tokenService,
            IHubDispatcher hubDispatcher,
            IHubContext<OperatorHub> operatorHubContext,
            IOperatorLogService logService,
            ILogger<ChatHub> logger,
            Mapper mapper)
        : base(userService, hubDispatcher, messageDialogService, messageService)
        {
            _tokenService = tokenService;
            _operatorHubContext = operatorHubContext;
            _logService = logService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Send(Message message)
        {
            try
            {
                var user = await GetUser();
                message.Sender = user.Id;
                message.Time = DateTime.UtcNow;

                DialogGroup dialogGroup;
                if (message.MessageDialogId.IsEmpty())
                {
                    dialogGroup = await HubDispatcher.CreateGroup(user, Context.ConnectionId);
                    message.MessageDialogId = dialogGroup.MessageDialogId;
                    await Groups.AddToGroupAsync(Context.ConnectionId, dialogGroup.Name);
                    await _operatorHubContext.Clients.All.SendAsync("dialogCreated", dialogGroup.MessageDialogId);
                }
            
                dialogGroup = HubDispatcher.GetDialogGroup(message.MessageDialogId);
                if (dialogGroup == null)
                    throw new InvalidOperationException($"Group by message dialog id '{message.MessageDialogId}' not found");
                if (!dialogGroup.UserExist(user))
                {
                    throw new InvalidOperationException($"User {user.Login} not connected in chat room");
                }

                message.Status = MessageStatus.Saved;
                message = await MessageService.Add(message);
                dialogGroup.LastMessageTime = message.Time;

                await Clients.Caller.SendAsync("meta", _mapper.Map<MessageResponse>(message));

                if (dialogGroup.MemberCount > 1)
                {
                    await Clients.Clients(dialogGroup
                            .Others(user.Id)
                            .Select(_ => _.ConnectionId))
                        .SendAsync("send", _mapper.Map<MessageResponse>(message));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        [CustomSecurity(SecurityPolicy.OperatorConnect)]
        public async Task OperatorConnect(Guid messageDialogId)
        {
            var user = await GetUser();
            var dialogGroup = HubDispatcher.GetDialogGroup(messageDialogId);
            dialogGroup.AddUser(user, Context.ConnectionId, true);
            await _logService.Log(user.Id, $"Operator {user.Login} connect to dialog {messageDialogId}");
            await Clients.Caller.SendAsync("operatorConnect", "success");
        }

        public override async Task OnConnectedAsync()
        {
            await CheckDeprecated();
            await Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} open");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await GetUser();
            DialogGroup[] dialogGroups = HubDispatcher.GetDialogGroups(Context.ConnectionId);
            bool isOperatorConnection = HubDispatcher.CheckOperator(user);
            foreach (var dialogGroup in dialogGroups)
            {
                dialogGroup.RemoveUser(Context.ConnectionId);
            }

            await CheckDeprecated();

            if (isOperatorConnection)
                await _logService.Log(user.Id,
                    $"Operator disconnect from {string.Join(',', dialogGroups.Select(_ => _.MessageDialogId))}");
        }


        protected override Task NotifyOperators(Guid messageDialogId)
        {
            return _operatorHubContext.Clients.All.SendAsync("dialogClosed", messageDialogId);
        }
    }
}