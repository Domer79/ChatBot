using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class TokenRepository: ITokenRepository
    {
        private readonly ChatbotContext _context;

        public TokenRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<Token> GetTokenById(string tokenId)
        {
            return _context.Tokens.SingleOrDefaultAsync(_ => _.TokenId == tokenId);
        }

        public async Task<Token> Add(Token token)
        {
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }
    }
}