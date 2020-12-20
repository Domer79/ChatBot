using System.Threading.Tasks;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> GetPermission(SecurityPolicy securityPolicy);
        Task<Permission[]> GetAll();
        Task<Permission> Upsert(Permission permission);
        Task<bool> Delete(Permission permission);
        Task<bool> Delete(SecurityPolicy policy);
    }
}