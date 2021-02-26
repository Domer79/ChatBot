using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public class HubDispatcher : IHubDispatcher, IDisposable
    {
        private readonly DialogActiveCollection _dialogActiveCollection;
        private readonly IMessageDialogService _messageDialogService;
        private readonly UserSet _userSet;
        private readonly IHubContext<OperatorHub> _operatorHub;
        private readonly IAppConfig _appconfig;
        private Timer _timer;

        public HubDispatcher( 
            DialogActiveCollection dialogActiveCollection,
            IMessageDialogService messageDialogService,
            UserSet userSet,
            IHubContext<OperatorHub> operatorHub,
            IAppConfig appconfig
        )
        {
            _dialogActiveCollection = dialogActiveCollection;
            _messageDialogService = messageDialogService;
            _userSet = userSet;
            _operatorHub = operatorHub;
            _appconfig = appconfig;
            // CheckDeprecated().GetAwaiter().GetResult();
            // InitCheckDeprecated();
        }

        private void InitCheckDeprecated()
        {
            _timer = new Timer(_appconfig.Chat.DecayTime.Milliseconds);
            _timer.Elapsed += CheckDeprecateEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void CheckDeprecateEvent(object sender, ElapsedEventArgs e)
        {
            CheckDeprecated().GetAwaiter().GetResult();
        }

        public Task<DialogGroup> GetDialogGroup(Guid messageDialogId)
        {
            return _dialogActiveCollection.GetDialogGroup(messageDialogId);
        }

        public Task<DialogGroup> GetOrCreateDialogGroup(User user, Guid messageDialogId)
        {
            return _dialogActiveCollection.GetOrCreateDialog(user, messageDialogId);
        }

        public DialogGroup[] GetDeprecated()
        {
            return _dialogActiveCollection.GetDeprecated();
        }

        public async Task CloseClientDialog(Guid userId, Guid? messageDialogId = null)
        {
            var dialogGroups = _dialogActiveCollection.Where(_ => _.ClientId == userId).ToArray();
            if (dialogGroups.Length == 0 && messageDialogId.HasValue)
            {
                dialogGroups = _dialogActiveCollection.Where(_ => _.MessageDialogId == messageDialogId).ToArray();
            }
            foreach (var dialogGroup in dialogGroups)
            {
                await _messageDialogService.Close(dialogGroup.MessageDialogId);
                await BroadcastOperators("dialogClosed", dialogGroup.MessageDialogId);
                _dialogActiveCollection.CloseDialogGroup(dialogGroup);
            }
        }

        private async Task CheckDeprecated()
        {
            var dialogGroups = GetDeprecated();
            foreach (var dialogGroup in dialogGroups)
            {
                await _messageDialogService.Close(dialogGroup.MessageDialogId);
                await BroadcastOperators("dialogClosed", dialogGroup.MessageDialogId);
            }

            _userSet.RemoveInactiveUsers();
        }

        private Task BroadcastOperators(string methodName, object arg1)
        {
            return _operatorHub.Clients.All.SendAsync(methodName, arg1);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}