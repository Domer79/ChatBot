using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Core
{
    public class AuthService: IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IRoleService _roleService;

        public AuthService(IUserService userService, 
            ITokenService tokenService,
            IRoleService roleService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _roleService = roleService;
        }

        public async Task<Token> LogIn(string loginOrEmail, string password)
        {
            if (!await _userService.ValidatePassword(loginOrEmail, password))
                return null;

            var user = await _userService.GetByLoginOrEmail(loginOrEmail);
            return await _tokenService.IssueToken(user);
        }

        public async Task<bool> CheckAccess(SecurityPolicy policy, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            var roles = await _userService.GetRoles(user.Id);
            if (roles == null || roles.Length == 0)
                return false;

            var permissions = await _roleService.GetPermissions(roles.Select(_ => _.Id).ToArray());
            return permissions.Any(_ => (SecurityPolicy) _.Politic == policy);
        }

        public async Task<bool> CheckAccess(SecurityPolicy policy, Guid userId)
        {
            var user = await _userService.GetById(userId);
            return await CheckAccess(policy, user);
        }

        public async Task<bool> CheckAccess(SecurityPolicy policy, string loginOrEmail)
        {
            var user = await _userService.GetByLoginOrEmail(loginOrEmail);
            return await CheckAccess(policy, user);
        }

        public Task<bool> ValidateToken(string tokenId)
        {
            return _tokenService.ValidateToken(tokenId);
        }

        public async Task<User> GetUserByToken(string tokenId)
        {
            var token = await _tokenService.GetToken(tokenId);
            var user = await _userService.GetById(token.UserId);

            return user;
        }
    }
}