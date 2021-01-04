using System;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Contracts.Chat
{
    /// <summary>
    /// Информация по статусу сообщения
    /// </summary>
    public class MessageInfo
    {
        public Guid Id { get; set; }
        public Guid MessageDialogId { get; set; }
        public MessageStatus Status { get; set; }
    }
}