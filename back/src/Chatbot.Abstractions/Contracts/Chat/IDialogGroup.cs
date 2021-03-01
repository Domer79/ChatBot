using System;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Contracts.Chat
{
    public interface IDialogGroup
    {
        void AddUser(User user);
        void RemoveUser(User user);
        Guid MessageDialogId { get; }
        Guid ClientId { get; set; }
        int MemberCount { get; }
        DateTime LastMessageTime { get; set; }
        bool IsDeprecated { get; }

        /// <summary>
        /// Возвращает пользовательского клиента, при этом клиент в диалоге должен быть только один
        /// </summary>
        UserConnection ClientConnection { get; }

        bool UserExist(User user);
        string[] Others(Guid userId);
        string Caller(Guid userId);
        string[] All();
    }
}