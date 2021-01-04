using System;

namespace Chatbot.Abstractions.Contracts.Chat
{
    public class DialogUser
    {
        public Guid UserId { get; set; }
        public bool IsOperator { get; set; }
    }
}