using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface IRoleRepository
    {
        Task<Permission[]> GetPermissions(Guid[] roleIds);
        Task<Role[]> GetAll();
    }
}