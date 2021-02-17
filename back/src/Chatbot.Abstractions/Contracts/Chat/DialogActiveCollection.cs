using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Contracts.Chat
{
    public class DialogActiveCollection: IEnumerable<DialogGroup>
    {
        private readonly IMessageDialogService _dialogService;
        private readonly IAppConfig _appConfig;
        private readonly UserSet _userSet;
        private readonly Lazy<Dictionary<Guid, DialogGroup>> _dialogGroups;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public DialogActiveCollection(
            IMessageDialogService dialogService,
            IAppConfig appConfig,
            UserSet userSet)
        {
            _dialogService = dialogService;
            _appConfig = appConfig;
            _userSet = userSet;

            _dialogGroups = new Lazy<Dictionary<Guid, DialogGroup>>(() => InitDialogGroups(dialogService, userSet, appConfig));
        }

        public async Task<DialogGroup> GetOrCreateDialog(User user, Guid messageDialogId)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!_dialogGroups.Value.TryGetValue(messageDialogId, out var dialogGroup))
                {
                    if (messageDialogId != Guid.Empty)
                        throw new InvalidCastException("Dialog either not found or rejected");
                
                    if (messageDialogId == Guid.Empty)
                    {
                        var messageDialog = await _dialogService.Start(user.Id);
                        dialogGroup = new DialogGroup(messageDialog, _userSet, _appConfig.Chat);
                        dialogGroup.AddUser(user);
                        _dialogGroups.Value.Add(messageDialog.Id, dialogGroup);
                        return dialogGroup;
                    }
                }

                dialogGroup.AddUser(user);
                return dialogGroup;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<DialogGroup> GetDialogGroup(Guid messageDialogId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var dialogGroup = _dialogGroups.Value[messageDialogId];
                if (dialogGroup != null) 
                    return dialogGroup;
            
                var dialog = await _dialogService.GetDialog(messageDialogId);
                dialogGroup = new DialogGroup(dialog, _userSet, _appConfig.Chat);
                _dialogGroups.Value.Add(messageDialogId, dialogGroup);

                return dialogGroup;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public DialogGroup[] GetDeprecated()
        {
            return _dialogGroups.Value.Values.Where(_ => _.IsDeprecated).ToArray();
        }

        public IEnumerator<DialogGroup> GetEnumerator()
        {
            return _dialogGroups.Value.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private static Dictionary<Guid, DialogGroup> InitDialogGroups(IMessageDialogService messageDialogService, 
            UserSet userSet, IAppConfig appConfig)
        {
            var dialogs = messageDialogService.GetByStatusFlags(DialogStatus.Started | DialogStatus.Active)
                .GetAwaiter().GetResult();
            return dialogs.ToDictionary(_ => _.Id, _ => new DialogGroup(_, userSet, appConfig.Chat));
        }

        public void CloseDialogGroup(DialogGroup dialogGroup)
        {
            _dialogGroups.Value.Remove(dialogGroup.MessageDialogId);
        }
    }
}