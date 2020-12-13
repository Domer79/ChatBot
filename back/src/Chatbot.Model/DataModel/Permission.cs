using System;
using System.Collections.Generic;

namespace Chatbot.Model.DataModel
{
    public class Permission
    {
        public Guid Id { get; set; }
        public int Politic { get; set; }
        public DateTime DateCreated { get; set; }
        public HashSet<Role> Roles { get; set; }
        public HashSet<RolePermission> PermissionRoles { get; set; }
    }
}