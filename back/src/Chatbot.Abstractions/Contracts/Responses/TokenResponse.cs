using System;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class TokenResponse
    {
        public string TokenId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpired { get; set; }
        public TimeSpan AutoExpired { get; set; }
        public Guid UserId { get; set; }
    }
}