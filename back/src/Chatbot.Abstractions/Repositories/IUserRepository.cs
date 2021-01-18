using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<User[]> GetAll();
        Task<User> GetById(Guid id);
        Task<User> Upsert(User user);
        Task<bool> Remove(Guid userId);
        Task<bool> Remove(User user);
        Task<User> GetByLoginOrEmail(string loginOrEmail);
        Task<Role[]> GetRoles(Guid userId);
        Task<Role[]> GetRoles(string loginOrEmail);
        Task<User[]> GetByIds(Guid[] ids);
        Task<User[]> GetPage(int pageNumber, int pageSize, bool? isActive);
        Task<long> GetTotalCount(bool? isActive);
    }
}