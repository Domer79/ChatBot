using System;

namespace Chatbot.Model.DataModel
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public DateTime DateCreated { get; set; }
        
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}