using System;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fio { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateBlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsOperator { get; set; }
    }
}