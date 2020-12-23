using System;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public MessageOwner Owner { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime Time { get; set; }
        public Guid MessageDialogId { get; set; }
        public Guid? Sender { get; set; }
    }
}