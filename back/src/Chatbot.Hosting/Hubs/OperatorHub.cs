using System;
using System.Threading;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
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
    public class OperatorHub : HubBase
    {
        private readonly Mapper _mapper;
        private readonly IOperatorLogService _logService;

        public OperatorHub(IHubDispatcher hubDispatcher,
            IOperatorLogService logService,
            IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService,
            UserSet userSet,
            ILogger<OperatorHub> logger,
            Mapper mapper)
            : base(userService, hubDispatcher, messageDialogService, messageService, userSet, logger)
        {
            _mapper = mapper;
            _logService = logService;
        }

        public async Task TakeToWork(MessageDialog dialog)
        {
            var dialogOperator = await GetUser();
            await _logService.Log(dialogOperator.Id, $"Dialog {dialog.Id} taken to work");
            dialog.OperatorId = dialogOperator.Id;
            await MessageDialogService.Activate(dialog);

            await Clients.Others.SendAsync("dialogTaken", dialog.Id, dialogOperator.Id);
        }
        //
        // protected override async Task SendMeta(MessageInfo messageInfo)
        // {
        //     var dialogGroup = await HubDispatcher.GetDialogGroup(messageInfo.MessageDialogId);
        //     await Clients.Clients(dialogGroup.Others(User.Id)).SendAsync("setMeta", messageInfo);
        // }
        //
        // protected override async Task SendOf(Message message)
        // {
        //     var dialogGroup = await HubDispatcher.GetDialogGroup(message.MessageDialogId);
        //     if (dialogGroup == null)
        //         throw new InvalidOperationException(
        //             $"Group by message dialog id '{message.MessageDialogId}' not found");
        //     if (!dialogGroup.UserExist(User))
        //     {
        //         dialogGroup.AddUser(User);
        //     }
        //
        //     message.Status = MessageStatus.Saved;
        //     message = await MessageService.Add(message);
        //     dialogGroup.LastMessageTime = message.Time;
        //     
        //     await Clients.Caller.SendAsync("setMeta", message);
        //
        //     if (dialogGroup.MemberCount > 1)
        //     {
        //         await Clients.Clients(dialogGroup.Others(User.Id))
        //             .SendAsync("send", _mapper.Map<MessageResponse>(message));
        //     }
        // }
    }
}