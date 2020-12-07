using System;

namespace Chatbot.Model.Data
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public MessageOwner Owner { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime Time { get; set; }
    }
}