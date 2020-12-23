using System;
using System.Collections.Generic;
using System.Linq;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Contracts
{
    public class DialogGroup
    {
        private readonly List<DialogUser> _dialogUsers = new();
        private readonly MessageDialog _dialog;
        private readonly string _name;

        public DialogGroup(MessageDialog dialog)
        {
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _name = Guid.NewGuid().ToString("N");
        }

        public void AddUser(User user, string connectionId, bool isOperator = false)
        {
            if (_dialogUsers.Any(_ => _.User.Id == user.Id)) return;
            
            _dialogUsers.Add(new DialogUser()
            {
                User = user,
                ConnectionId = connectionId,
                IsOperator = isOperator
            });
        }

        public void RemoveUser(User user)
        {
            var removedUser = _dialogUsers.FirstOrDefault(_ => _.User.Id == user.Id);
            if (removedUser == null) return;

            _dialogUsers.Remove(removedUser);
        }

        public void RemoveUser(string connectionId)
        {
            var dialogUser = _dialogUsers.Single(_ => _.ConnectionId == connectionId);
            _dialogUsers.Remove(dialogUser);
        }

        public string[] GetAllConnectionIds()
        {
            return _dialogUsers.Select(_ => _.ConnectionId).ToArray();
        }

        public string[] GetConnectionIdExcept(User user)
        {
            return _dialogUsers.Where(_ => _.User.Id != user.Id).Select(_ => _.ConnectionId).ToArray();
        }

        public string[] GetConnectionIdExcept(string connectionId)
        {
            return _dialogUsers.Where(_ => _.ConnectionId != connectionId).Select(_ => _.ConnectionId).ToArray();
        }

        public string Name => _name;
        public Guid MessageDialogId => _dialog.Id;
        public int MemberCount => _dialogUsers.Count;
        public DateTime LastMessageTime { get; set; }
        
        public bool IsDeprecated => (DateTime.Now - LastMessageTime) > TimeSpan.FromMinutes(10);

        public bool UserExist(User user)
        {
            return _dialogUsers.Any(_ => _.User.Id == user.Id);
        }

        public bool IsOperator(User user)
        {
            var dialogUser = _dialogUsers.FirstOrDefault(_ => _.User.Id == user.Id);
            return dialogUser?.IsOperator ?? throw new InvalidOperationException("Not found");
        }
    }
}