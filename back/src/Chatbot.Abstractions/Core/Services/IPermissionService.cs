using System.Threading.Tasks;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IPermissionService
    {
        /// <summary>
        /// Обновление политик безопасности в БД
        /// </summary>
        /// <returns></returns>
        Task RefreshPolicy();
    }
}