using System;

namespace Chatbot.Model.DataModel
{
    public class Token
    {
        public string TokenId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpired { get; set; }
        public TimeSpan AutoExpired { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}