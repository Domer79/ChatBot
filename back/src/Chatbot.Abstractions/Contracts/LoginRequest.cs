using System;

namespace Chatbot.Abstractions.Contracts
{
    public class LoginRequest
    {
        public string LoginOrEmail { get; set; }
        public string Password { get; set; }
    }

    public class PasswordRequest
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}