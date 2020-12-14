using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IUserService
    {
        Task<User[]> GetAll();
        Task<User> GetById(Guid id);
        Task<User> GetByLoginOrEmail(string loginOrEmail);
        Task<User> Upsert(User user);
        Task<bool> Remove(Guid userId);
        Task Remove(User user);
        Task<bool> ValidatePassword(Guid userId, string password);
        Task<bool> ValidatePassword(string loginOrEmail, string password);
        bool ValidatePassword(User user, string password);
        Task<bool> SetPassword(Guid userId, string password);
        Task<bool> SetPassword(string loginOrEmail, string password);
        Task<bool> SetPassword(User user, string password);
        Task<Role[]> GetRoles(Guid userId);
        Task<Role[]> GetRoles(string loginOrEmail);
    }
}