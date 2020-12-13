using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core
{
    public interface IAuthService
    {
        Task<Token> LogIn(string loginOrEmail, string password);
        Task<bool> CheckAccess(User user);
        Task<bool> CheckAccess(Guid userId);
        Task<bool> CheckAccess(string loginOrEmail);
        Task<bool> ValidateToken(string tokenId);
    }
}