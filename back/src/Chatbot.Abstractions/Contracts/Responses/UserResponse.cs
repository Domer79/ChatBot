using System;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }
}