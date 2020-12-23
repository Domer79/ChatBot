using System;
using System.Collections.Generic;
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
    public class HubDispatcher : IHubDispatcher, IDialogCreated
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMessageService _messageService;
        private readonly List<DialogGroup> _dialogGroups = new();
        private Func<Guid, Task> _dialogCreated; 

        public HubDispatcher( 
            IMessageDialogService messageDialogService,
            IMessageService messageService
        )
        {
            _messageDialogService = messageDialogService;
            _messageService = messageService;
        }

        public DialogGroup GetDialogGroup(Guid messageDialogId)       
        {
            return _dialogGroups.SingleOrDefault(_ => _.MessageDialogId == messageDialogId);
        }

        public async Task<DialogGroup> CreateGroup(User user, string connectionId)
        {
            var messageDialog = await _messageDialogService.Start(user.Id);
            var dialogGroup = new DialogGroup(messageDialog);
            dialogGroup.AddUser(user, connectionId);
            _dialogGroups.Add(dialogGroup);
            return dialogGroup;
        }

        public void ConfigureDialogCreated(Func<Guid, Task> dialogCreated)
        {
            _dialogCreated = dialogCreated;
        }

        public async Task RemoveDialogGroup(DialogGroup dialogGroup)
        {
            _dialogGroups.Remove(dialogGroup);
            await _messageDialogService.Close(dialogGroup.MessageDialogId);
        }

        public Task DialogCreated(Guid messageDialogId)
        {
            return _dialogCreated?.Invoke(messageDialogId);
        }

        public DialogGroup[] GetDialogGroups(string connectionId)
        {
            var list = new List<DialogGroup>();
            foreach (var dialogGroup in _dialogGroups)
            {
                if (dialogGroup.GetAllConnectionIds().Any(_ => _ == connectionId))
                    list.Add(dialogGroup);
            }

            return list.ToArray();
        }

        public DialogGroup[] GetDialogGroups()
        {
            return _dialogGroups.ToArray();
        }

        public bool CheckOperator(User user)
        {
            foreach (var dialogGroup in _dialogGroups)
            {
                if (dialogGroup.UserExist(user) && dialogGroup.IsOperator(user))
                    return true;
            }

            return false;
        }
    }
}