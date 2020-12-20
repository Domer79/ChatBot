using System;

namespace Chatbot.Model.DataModel
{
    public class OperatorLog
    {
        public Guid Id { get; set; }
        public Guid OperatorId { get; set; }
        public string Action { get; set; }
        public DateTime DateCreated { get; set; }
    }
}