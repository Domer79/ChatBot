using System;
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

        public async Task CloseClientDialog(Guid userId)
        {
            foreach (var dialogGroup in _dialogActiveCollection)
            {
                if (dialogGroup.ClientId == userId)
                {
                    await _messageDialogService.Close(dialogGroup.MessageDialogId);
                    await NotifyOperators(dialogGroup.MessageDialogId);
                }
            }
        }

        private async Task CheckDeprecated()
        {
            var dialogGroups = GetDeprecated();
            foreach (var dialogGroup in dialogGroups)
            {
                await _messageDialogService.Close(dialogGroup.MessageDialogId);
                await NotifyOperators(dialogGroup.MessageDialogId);
            }

            _userSet.RemoveInactiveUsers();
        }
        
        public Task NotifyOperators(Guid messageDialogId)
        {
            return _operatorHub.Clients.All.SendAsync("dialogClosed", messageDialogId);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}