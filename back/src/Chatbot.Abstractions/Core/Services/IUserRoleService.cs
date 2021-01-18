using System;
using System.Threading.Tasks;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IUserRoleService
    {
        Task Add(Guid userId, Guid roleId);
        Task<bool> Delete(Guid userId, Guid roleId);
        Task<bool> Check(Guid userId, Guid roleId);
    }
}