using System;

namespace Chatbot.Abstractions.Contracts.Requests
{
    public class AuthDataRequest
    {
        public string Fio { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}