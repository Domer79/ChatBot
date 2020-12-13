using System;

namespace Chatbot.Model.DataModel
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        public DateTime DateCreated { get; set; } 
    }
}