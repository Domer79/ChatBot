using System.Security.Principal;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface ITokenRepository
    {
        Task<Token> GetTokenById(string tokenId);
        Task<Token> Add(Token token);
    }
}