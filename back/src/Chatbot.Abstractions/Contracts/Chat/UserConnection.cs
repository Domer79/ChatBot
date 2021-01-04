using System;

namespace Chatbot.Abstractions.Contracts.Chat
{
    public class UserConnection
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
        
        public string DialogConnectionId { get; set; }
        
        public DateTime LastActivity { get; set; }
        
        public bool IsOperator { get; set; }

        public void RefreshActivity()
        {
            LastActivity = DateTime.Now;
        }

        public bool CheckIsNotActivity(TimeSpan decayTime)
        {
            return (DateTime.Now - LastActivity) > decayTime;
        }
    }
}