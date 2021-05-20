using System;
using Chatbot.Model.Enums;

namespace Chatbot.Model.DataModel
{
    public class Stat
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid? QuestionId { get; set; }
        
        public DateTime Time { get; set; }
        
        public StatType Type { get; set; }
    }
}