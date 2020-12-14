using System;
using System.Collections.Generic;

namespace Chatbot.Model.DataModel
{
    public class MessageDialog
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateWork { get; set; }
        public DateTime DateCompleted { get; set; }
        public int DialogStatus { get; set; } 
        public HashSet<Message> Messages { get; set; }
    }
}