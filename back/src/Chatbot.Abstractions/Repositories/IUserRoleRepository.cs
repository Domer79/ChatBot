using System;
using System.Threading.Tasks;

namespace Chatbot.Abstractions.Repositories
{
    public interface IUserRoleRepository
    {
        Task Add(Guid userId, Guid roleId);
        Task<bool> Delete(Guid userId, Guid roleId);
    }
}