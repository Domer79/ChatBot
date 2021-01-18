using System;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class UserRoleResponse : RoleResponse
    {
        public bool Setted { get; set; }
    }
}