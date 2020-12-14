using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Enums;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core
{
    public interface IAuthService
    {
        Task<Token> LogIn(string loginOrEmail, string password);
        Task<bool> CheckAccess(SecurityPolicy policy, User user);
        Task<bool> CheckAccess(SecurityPolicy policy, Guid userId);
        Task<bool> CheckAccess(SecurityPolicy policy, string loginOrEmail);
        Task<bool> ValidateToken(string tokenId);
        Task<User> GetUserByToken(string tokenId);
    }
}