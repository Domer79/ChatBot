using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Core.Exceptions;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Core.Chat
{
    public class DialogActiveCollection: IEnumerable<IDialogGroup>
    {
        private readonly IMessageDialogService _dialogService;
        private readonly IAppConfig _appConfig;
        private readonly UserSet _userSet;
        private readonly Lazy<Dictionary<Guid, IDialogGroup>> _dialogGroups;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public DialogActiveCollection(
            IMessageDialogService dialogService,
            IAppConfig appConfig,
            UserSet userSet)
        {
            _dialogService = dialogService;
            _appConfig = appConfig;
            _userSet = userSet;

            _dialogGroups = new Lazy<Dictionary<Guid, IDialogGroup>>(() => InitDialogGroups(dialogService, userSet, appConfig));
        }

        public async Task<IDialogGroup> GetOrCreateDialog(User user, Guid messageDialogId)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!_dialogGroups.Value.TryGetValue(messageDialogId, out var dialogGroup))
                {
                    MessageDialog messageDialog;
                    if (messageDialogId != Guid.Empty)
                    {
                        var dialog = await _dialogService.GetDialog(messageDialogId);
                        if (dialog == null) throw new InvalidOperationException("Dialog not found");
                        if (!(DialogStatus.Closed | DialogStatus.Rejected).HasFlag(dialog.DialogStatus))
                            throw new InvalidOperationException(
                                "When dialog creating based prev dialog, this dialog must by closed or rejected");

                        messageDialog = await _dialogService.Start(user.Id, messageDialogId);
                    }
                    else
                    {
                        messageDialog = await _dialogService.Start(user.Id);
                    }
                    
                    dialogGroup = new DialogGroup(messageDialog, _userSet, _appConfig.Chat);
                    dialogGroup.AddUser(user);
                    _dialogGroups.Value.Add(messageDialog.Id, dialogGroup);
                    return dialogGroup;
                }

                dialogGroup.AddUser(user);
                return dialogGroup;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IDialogGroup> GetDialogGroup(Guid messageDialogId)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_dialogGroups.Value.TryGetValue(messageDialogId, out var dialogGroup))
                {
                    return dialogGroup;
                }
            
                var dialog = await _dialogService.GetDialog(messageDialogId);
                if (DialogStatus.NotActive.HasFlag(dialog.DialogStatus))
                {
                    throw new DialogNotActiveException(dialog.Id, dialog.Number, dialog.DialogStatus);
                }
                
                dialogGroup = new DialogGroup(dialog, _userSet, _appConfig.Chat);
                _dialogGroups.Value.Add(messageDialogId, dialogGroup);

                return dialogGroup;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public IDialogGroup[] GetDeprecated()
        {
            return _dialogGroups.Value.Values.Where(_ => _.IsDeprecated).ToArray();
        }

        public IEnumerator<IDialogGroup> GetEnumerator()
        {
            return _dialogGroups.Value.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private static Dictionary<Guid, IDialogGroup> InitDialogGroups(IMessageDialogService messageDialogService, 
            UserSet userSet, IAppConfig appConfig)
        {
            var dialogs = messageDialogService.GetByStatusFlags(DialogStatus.Started | DialogStatus.Active)
                .GetAwaiter().GetResult();
            return dialogs.ToDictionary(_ => _.Id, _ => (IDialogGroup) new DialogGroup(_, userSet, appConfig.Chat));
        }

        public void CloseDialogGroup(IDialogGroup dialogGroup)
        {
            _dialogGroups.Value.Remove(dialogGroup.MessageDialogId);
        }
    }
}