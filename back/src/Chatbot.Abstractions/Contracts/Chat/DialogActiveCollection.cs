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
        private readonly IMessageService _messageService;
        private readonly IAppConfig _appConfig;
        private readonly UserSet _userSet;
        private readonly Lazy<List<DialogGroup>> _dialogGroups;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public DialogActiveCollection(
            IMessageDialogService dialogService,
            IMessageService messageService,
            IAppConfig appConfig,
            UserSet userSet)
        {
            _dialogService = dialogService;
            _messageService = messageService;
            _appConfig = appConfig;
            _userSet = userSet;

            _dialogGroups = new Lazy<List<DialogGroup>>(() => InitDialogGroups(dialogService, userSet, appConfig));
        }

        public async Task<DialogGroup> GetOrCreateDialog(User user, Guid messageDialogId)
        {
            DialogGroup dialogGroup;
            if (messageDialogId == Guid.Empty)
            {
                var messageDialog = await _dialogService.Start(user.Id);
                messageDialogId = messageDialog.Id;
                dialogGroup = new DialogGroup(messageDialog, _userSet, _appConfig.Chat);
                dialogGroup.AddUser(user);
                // _dialogGroups.Value.Add(dialogGroup);
                // return dialogGroup;
            }

            dialogGroup = await GetDialogGroup(messageDialogId);
            dialogGroup.AddUser(user);
            return dialogGroup;
        }

        public async Task<DialogGroup> GetDialogGroup(Guid messageDialogId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var dialogGroup = _dialogGroups.Value.SingleOrDefault(_ => _.MessageDialogId == messageDialogId);
                if (dialogGroup != null) 
                    return dialogGroup;
            
                var dialog = await _dialogService.GetDialog(messageDialogId);
                var lastMessages = await _messageService.GetLastMessages(new []{ dialog.Id });
                dialogGroup = new DialogGroup(dialog, _userSet, _appConfig.Chat, lastMessages[0].Time);
                _dialogGroups.Value.Add(dialogGroup);

                return dialogGroup;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public DialogGroup[] GetDeprecated()
        {
            return _dialogGroups.Value.Where(_ => _.IsDeprecated).ToArray();
        }

        public IEnumerator<DialogGroup> GetEnumerator()
        {
            return _dialogGroups.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private static List<DialogGroup> InitDialogGroups(IMessageDialogService messageDialogService, 
            UserSet userSet, IAppConfig appConfig)
        {
            var dialogs = messageDialogService.GetByStatusFlags(DialogStatus.Started | DialogStatus.Active)
                .GetAwaiter().GetResult();
            return dialogs.Select(_ => new DialogGroup(_, userSet, appConfig.Chat)).ToList();
        }
    }
}