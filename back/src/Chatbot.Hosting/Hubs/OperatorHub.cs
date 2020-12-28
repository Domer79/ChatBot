using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Hosting.Authentication;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    [Authorize]
    public class OperatorHub : HubBase
    {
        private readonly IOperatorLogService _logService;

        public OperatorHub(IHubDispatcher hubDispatcher,
            IOperatorLogService logService,
            IMessageService messageService,
            IMessageDialogService messageDialogService,
            IUserService userService)
            : base(userService, hubDispatcher, messageDialogService, messageService)
        {
            _logService = logService;
            
            HubDispatcher.ConfigureDialogCreated(DialogCreated);
        }

        private Task DialogCreated(Guid messageDialogId)
        {
            return Clients.All.SendAsync("dialogCreated", messageDialogId);
        }

        public async Task TakeToWork(MessageDialog dialog)
        {
            var dialogOperator = await GetUser();
            await _logService.Log(dialogOperator.Id, $"Dialog {dialog.Id} taken to work");
            dialog.OperatorId = dialogOperator.Id;
            await MessageDialogService.Activate(dialog);

            await Clients.Others.SendAsync("dialogTaken", dialog.Id, dialogOperator.Id);
        }

        public override async Task OnConnectedAsync()
        {
            await CheckDeprecated();
            await Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} open");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await CheckDeprecated();
            await Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} closed");
        }

        protected override Task NotifyOperators(Guid messageDialogId)
        {
            return Clients.All.SendAsync("dialogClosed", messageDialogId);
        }
    }
}