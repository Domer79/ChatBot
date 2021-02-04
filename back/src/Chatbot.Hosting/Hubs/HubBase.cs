using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting.Hubs
{
    public abstract class HubBase: Hub
    {
        private User _user;
        private readonly IUserService _userService;
        protected readonly IHubDispatcher HubDispatcher;
        protected readonly IMessageDialogService MessageDialogService;
        protected readonly IMessageService MessageService;
        protected readonly UserSet UserSet;
        private readonly ILogger _logger;

        protected HubBase(
            IUserService userService, 
            IHubDispatcher hubDispatcher,
            IMessageDialogService messageDialogService,
            IMessageService messageService,
            UserSet userSet,
            ILogger logger)
        {
            _userService = userService;
            HubDispatcher = hubDispatcher;
            MessageDialogService = messageDialogService;
            MessageService = messageService;
            UserSet = userSet;
            _logger = logger;
        }

        protected User User => _user ??= GetUser().GetAwaiter().GetResult();

        protected Task<User> GetUser()
        {
            var login = Context.User.GetLogin();
            return _userService.GetByLogin(login);
        }

        public async Task Send(Message message)
        {
            try
            {
                message.Sender = User.Id;
                message.Time = DateTime.UtcNow;
                UserSet[User.Id].LastActivity = DateTime.Now;
                await SendOf(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} open");
            _logger.LogInformation($"Connection {Context.ConnectionId} open");
            UserSet[User.Id].ConnectionId = Context.ConnectionId;
            await OnConnected();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Console.Out.WriteLineAsync($"Connection {Context.ConnectionId} close");
            _logger.LogInformation($"Connection {Context.ConnectionId} close");
            UserSet[User.Id].ConnectionId = null;
            await OnDisconnected();
        }

        protected virtual Task OnConnected()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnDisconnected()
        {
            return Task.CompletedTask;
        }

        public async Task MessageRead(MessageInfo messageInfo)
        {
            await MessageService.SetStatus(messageInfo);
            await SendMeta(messageInfo);
        }

        protected virtual Task SendMeta(MessageInfo messageInfo)
        {
            return Task.CompletedTask;
        }

        protected virtual Task SendOf(Message message)
        {
            return Task.CompletedTask;
        }
    }
}