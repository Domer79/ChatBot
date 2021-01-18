using System;

namespace Chatbot.Abstractions.Contracts.Requests
{
    public class PasswordRequest
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}