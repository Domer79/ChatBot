using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Core
{
    public interface IAuthService
    {
        Task<Token> LogIn(string login, string password);
        Task<bool> CheckAccess(SecurityPolicy policy, User user);
        Task<bool> CheckAccess(SecurityPolicy policy, Guid userId);
        Task<bool> CheckAccess(SecurityPolicy policy, string login);
        Task<bool> ValidateToken(string tokenId);
        Task<User> GetUserByToken(string tokenId);
        Task<bool> CheckAccessByToken(SecurityPolicy securityPolicy, string tokenId);
    }
}