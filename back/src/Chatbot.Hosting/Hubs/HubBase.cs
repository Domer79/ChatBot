using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public abstract class HubBase: Hub
    {
        protected readonly IUserService UserService;
        protected readonly IHubDispatcher HubDispatcher;
        protected readonly IMessageDialogService MessageDialogService;
        protected readonly IMessageService MessageService;

        protected HubBase(
            IUserService userService, 
            IHubDispatcher hubDispatcher,
            IMessageDialogService messageDialogService,
            IMessageService messageService)
        {
            UserService = userService;
            HubDispatcher = hubDispatcher;
            MessageDialogService = messageDialogService;
            MessageService = messageService;
        }

        protected Task<User> GetUser()
        {
            var login = Context.User.GetLogin();
            return UserService.GetByLoginOrEmail(login);
        }
        
        protected async Task CheckDeprecated()
        {
            var dbDialogs = await MessageDialogService.GetByStatusFlags(DialogStatus.Active | DialogStatus.Started);
            var lastMessages = Array.Empty<Message>();
            if (dbDialogs.Length > 0)
                lastMessages = await MessageService.GetLastMessages(dbDialogs.Select(_ => _.Id).ToArray());
            var dbDialogGroups = dbDialogs.Select(_ =>
            {
                var lastMessage = lastMessages.SingleOrDefault(m => m.MessageDialogId == _.Id);
                var dialog = new DialogGroup(_, lastMessage?.Time ?? default);
                return dialog;
            });
            
            var dialogGroups = HubDispatcher.GetDialogGroups().Concat(dbDialogGroups)
                .Where(_ => _.IsDeprecated || _.MemberCount == 0).ToArray();
            foreach (var dialogGroup in dialogGroups)
            {
                await Clients.Clients(dialogGroup.GetAllConnectionIds())
                    .SendAsync("closeDialog", dialogGroup.MessageDialogId);

                await NotifyOperators(dialogGroup.MessageDialogId);
                await HubDispatcher.RemoveDialogGroup(dialogGroup);
            }
        }

        protected abstract Task NotifyOperators(Guid messageDialogId);
    }
}