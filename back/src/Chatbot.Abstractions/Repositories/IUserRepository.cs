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
    }
}