using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Pipe;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Core.Chat;
using Chatbot.Core.Common;
using Chatbot.Core.Exceptions;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Hubs.MessageHandlers;
using Chatbot.Hosting.Hubs.Pipes;
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
        private readonly IChatBotHelper _chatBotHelper;
        private readonly IMessagePipe _messagePipe;
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
            IChatBotHelper chatBotHelper,
            IMessagePipe messagePipe,
            Mapper mapper)
        : base(userService, hubDispatcher, messageDialogService, messageService, userSet, logger)
        {
            _tokenService = tokenService;
            _operatorHubContext = operatorHubContext;
            _logService = logService;
            _chatBotHelper = chatBotHelper;
            _messagePipe = messagePipe;
            _mapper = mapper;
        }

        protected override async Task SendMeta(MessageInfo messageInfo)
        {
            var dialogGroup = await HubDispatcher.GetActiveDialogGroup(messageInfo.MessageDialogId);
            await Clients.Clients(dialogGroup.Others(User.Id)).SendAsync("setMeta", messageInfo);
        }

        protected override async Task SendOf(Message message)
        {
            var messageDialog = await MessageDialogService.GetDialog(message.MessageDialogId);
            var isNewDialog = message.MessageDialogId == Guid.Empty
                              || DialogStatus.NotActive.HasFlag(messageDialog.DialogStatus); 
            var context = new MessageContext(message, User, this, isNewDialog);
            await _messagePipe.Start(context);
        }

        [CustomSecurity(SecurityPolicy.OperatorConnection)]
        public async Task OperatorConnect(Guid messageDialogId)
        {
            UserSet[User.Id].IsOperator = true;
            var dialogGroup = await HubDispatcher.GetActiveDialogGroup(messageDialogId);
            dialogGroup.AddUser(User);
            await _logService.Log(User.Id, $"Operator {User.Login} connect to dialog {messageDialogId}");
            await Clients.Caller.SendAsync("operatorConnect", "success");
        }

        [CustomSecurity(SecurityPolicy.OperatorConnection)]
        public async Task OperatorDisconnect(Guid messageDialogId)
        {
            try
            {
                var dialogGroup = await HubDispatcher.GetActiveDialogGroup(messageDialogId);
                dialogGroup.RemoveUser(User);
                await _logService.Log(User.Id, $"Operator {User.Login} disconnected from dialog {messageDialogId}");
            }
            catch (DialogNotActiveException e)
            {
                await _logService.Log(User.Id, $"Диалог не активен: {e.Message}");
            }
        }

        protected override async Task OnDisconnected()
        {
            //await HubDispatcher.CloseClientDialog(User.Id);
        }
    }
}