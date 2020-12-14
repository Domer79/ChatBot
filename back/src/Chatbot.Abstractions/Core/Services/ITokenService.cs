using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface ITokenService
    {
        Task<bool> ValidateToken(string tokenId);
        Task<Token> IssueToken(Guid userId);
        Task<Token> IssueToken(User user);
        Task<Token> GetToken(string tokenId);
        Task<bool> IsExpires(string tokenId);
        Task<Token> Refresh(string tokenId);
    }
}