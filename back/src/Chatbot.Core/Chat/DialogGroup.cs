using System;
using System.Collections.Generic;
using System.Linq;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Chat
{
    public class DialogGroup : IDialogGroup
    {
        private readonly List<User> _dialogUsers = new();
        private readonly MessageDialog _dialog;
        private readonly UserSet _userSet;
        private readonly ChatConfig _config;
        private readonly string _name;
        private static readonly object Lock = new object();

        public DialogGroup(MessageDialog dialog, UserSet userSet, ChatConfig config)
        {
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _userSet = userSet ?? throw new ArgumentNullException(nameof(userSet));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _name = Guid.NewGuid().ToString("N");
        }

        public DialogGroup(MessageDialog dialog, UserSet userSet, ChatConfig config, DateTime lastMessageTime)
            : this(dialog, userSet, config)
        {
            LastMessageTime = lastMessageTime;
        }

        public void AddUser(User user)
        {
            lock (Lock)
            {
                var dialogUser = _dialogUsers.FirstOrDefault(_ => _.Id == user.Id);
                if (dialogUser == null)
                {
                    _dialogUsers.Add(user);
                }
            }
        }

        public void RemoveUser(User user)
        {
            var removedUser = _dialogUsers.FirstOrDefault(_ => _.Id == user.Id);
            if (removedUser != null)
                _dialogUsers.Remove(removedUser);
        }

        public Guid MessageDialogId => _dialog.Id;
        public Guid ClientId { get; set; }
        public int MemberCount => _dialogUsers.Count;
        public DateTime LastMessageTime { get; set; }

        public bool IsDeprecated
        {
            get
            {
                if (_dialogUsers.Count == 0)
                    return true;

                return _dialogUsers.All(_ => _userSet[_.Id].CheckIsNotActivity(_config.DecayTime));
            }
        }

        public bool UserExist(User user)
        {
            return _dialogUsers.Any(_ => _.Id == user.Id);
        }

        public string[] Others(Guid userId)
        {
            return _dialogUsers
                .Where(_ => _.Id != userId)
                .Select(_ => _userSet[_.Id].ConnectionId)
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .ToArray();
        }

        public string Caller(Guid userId)
        {
            return _userSet[userId].ConnectionId;
        }

        public string[] All()
        {
            return _dialogUsers
                .Select(_ => _userSet[_.Id].ConnectionId)
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .ToArray();
        }

        /// <summary>
        /// Возвращает пользовательского клиента, при этом клиент в диалоге должен быть только один
        /// </summary>
        public UserConnection ClientConnection => _dialogUsers.Select(_ => _userSet[_.Id]).SingleOrDefault(_ => !_.IsOperator);
    }
}