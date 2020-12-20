using System.Threading.Tasks;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IPermissionService
    {
        Task RefreshPolicy();
    }
}