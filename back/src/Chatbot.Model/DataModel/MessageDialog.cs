using System;
using System.Collections.Generic;
using Chatbot.Model.Enums;

namespace Chatbot.Model.DataModel
{
    public class MessageDialog
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateWork { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DialogStatus DialogStatus { get; set; } 
        public Guid? OperatorId { get; set; }
        public Guid? ClientId { get; set; }
        public bool Offline { get; set; }
        public HashSet<Message> Messages { get; set; }
        
        public User Operator { get; set; }
        public User Client { get; set; }
    }
}