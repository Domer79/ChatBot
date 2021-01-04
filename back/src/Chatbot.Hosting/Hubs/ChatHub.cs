using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Chat;
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
        private readonly Mapper _mapper;

        public ChatHub(IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService,
            ITokenService tokenService,
            IHubDispatcher hubDispatcher,
            IHubContext<OperatorHub> operatorHubContext,
            IOperatorLogService logService,
            ILogger<ChatHub> logger,
            UserSet userSet,
            Mapper mapper)
        : base(userService, hubDispatcher, messageDialogService, messageService, userSet, logger)
        {
            _tokenService = tokenService;
            _operatorHubContext = operatorHubContext;
            _logService = logService;
            _mapper = mapper;
        }

        protected override async Task SendMeta(MessageInfo messageInfo)
        {
            var dialogGroup = await HubDispatcher.GetDialogGroup(messageInfo.MessageDialogId);
            await Clients.Clients(dialogGroup.Others(User.Id)).SendAsync("setMeta", messageInfo);
        }

        protected override async Task SendOf(Message message)
        {
            var dialogGroup = await HubDispatcher.GetOrCreateDialogGroup(User, message.MessageDialogId);
            if (message.MessageDialogId.IsEmpty())
            {
                message.MessageDialogId = dialogGroup.MessageDialogId;
                dialogGroup.ClientId = User.Id;
                await _operatorHubContext.Clients.All.SendAsync("dialogCreated", dialogGroup.MessageDialogId);
            }
        
            if (dialogGroup == null)
                throw new InvalidOperationException(
                    $"Group by message dialog id '{message.MessageDialogId}' not found");
            if (!dialogGroup.UserExist(User))
            {
                throw new InvalidOperationException($"User {User.Login} not connected in chat room");
            }
        
            message.Status = MessageStatus.Saved;
            message = await MessageService.Add(message);
            dialogGroup.LastMessageTime = message.Time;
            
            await Clients.Caller.SendAsync("setMeta", message);
        
            if (dialogGroup.MemberCount > 1)
            {
                await Clients.Clients(dialogGroup.Others(User.Id))
                    .SendAsync("send", _mapper.Map<MessageResponse>(message));
            }
        }

        [CustomSecurity(SecurityPolicy.OperatorConnection)]
        public async Task OperatorConnect(Guid messageDialogId)
        {
            UserSet[User.Id].IsOperator = true;
            var dialogGroup = await HubDispatcher.GetDialogGroup(messageDialogId);
            dialogGroup.AddUser(User);
            await _logService.Log(User.Id, $"Operator {User.Login} connect to dialog {messageDialogId}");
            await Clients.Caller.SendAsync("operatorConnect", "success");
        }

        [CustomSecurity(SecurityPolicy.OperatorConnection)]
        public async Task OperatorDisconnect(Guid messageDialogId)
        {
            var dialogGroup = await HubDispatcher.GetDialogGroup(messageDialogId);
            dialogGroup.RemoveUser(User);
            await _logService.Log(User.Id, $"Operator {User.Login} disconnected from dialog {messageDialogId}");
        }

        protected override async Task OnDisconnected()
        {
            await HubDispatcher.CloseClientDialog(User.Id);
        }
    }
}