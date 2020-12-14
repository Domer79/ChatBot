using System;

namespace Chatbot.Model.DataModel
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public MessageOwner Owner { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime Time { get; set; }
        public Guid MessageDialogId { get; set; }
        public Guid? Sender { get; set; }
        
        public MessageDialog Dialog { get; set; }
    }
}