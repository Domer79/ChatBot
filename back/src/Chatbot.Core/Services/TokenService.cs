using System;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class TokenService: ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAppConfig _appConfig;

        public TokenService(ITokenRepository tokenRepository, IAppConfig appConfig)
        {
            _tokenRepository = tokenRepository;
            _appConfig = appConfig;
        }

        public async Task<bool> ValidateToken(string tokenId)
        {
            var token = await _tokenRepository.GetTokenById(tokenId);
            if (token == null)
                return false;
            
            return token.DateExpired > DateTime.Now;
        }

        public async Task<Token> IssueToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var token = new Token()
            {
                TokenId = Helper.Generate(),
                DateExpired = DateTime.Now.AddHours(_appConfig.GetTokenLifetime()),
                AutoExpired = TimeSpan.FromMinutes(_appConfig.GetTokenAutoExpired()),
                UserId = user.Id
            };

            return await _tokenRepository.Add(token);
        }

        public Task<Token> GetToken(string tokenId)
        {
            return _tokenRepository.GetTokenById(tokenId);
        }
    }
}