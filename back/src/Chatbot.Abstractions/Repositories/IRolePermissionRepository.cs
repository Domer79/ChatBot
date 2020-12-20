using System;
using System.Threading.Tasks;

namespace Chatbot.Abstractions.Repositories
{
    public interface IRolePermissionRepository
    {
        Task Add(Guid roleId, Guid permissionId);
        Task<bool> Delete(Guid roleId, Guid permissionId);
    }
}