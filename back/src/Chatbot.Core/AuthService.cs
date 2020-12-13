using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Model.DataModel;

namespace Chatbot.Core
{
    public class AuthService: IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<Token> LogIn(string loginOrEmail, string password)
        {
            if (!await _userService.ValidatePassword(loginOrEmail, password))
                return null;

            var user = await _userService.GetByLoginOrEmail(loginOrEmail);
            return await _tokenService.IssueToken(user);
        }

        public Task<bool> CheckAccess(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckAccess(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckAccess(string loginOrEmail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateToken(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}