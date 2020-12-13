using System;
using System.Collections.Generic;

namespace Chatbot.Model.DataModel
{
    public class MessageDialog
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid OperatorId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateWork { get; set; }
        public DateTime DateCompleted { get; set; }
        public User Client { get; set; }
        public User Operator { get; set; }
        public HashSet<Message> Messages { get; set; }
    }
}