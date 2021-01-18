using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IRoleService
    {
        Task<Permission[]> GetPermissions(Guid[] roleIds);
        Task<Role[]> GetRoles();
        Task<bool> SetRole(Guid roleId, Guid userId, bool set);
    }
}