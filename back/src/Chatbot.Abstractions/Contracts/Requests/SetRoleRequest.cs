using System;

namespace Chatbot.Abstractions.Contracts.Requests
{
    public class SetRoleRequest
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public bool Set { get; set; }
    }
}