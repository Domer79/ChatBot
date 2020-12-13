using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface ITokenService
    {
        Task<bool> ValidateToken(string tokenId);
        Task<Token> IssueToken(User user);
    }
}