using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chatbot.Hosting.Authentication
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IAuthService authService, ITokenService tokenService, 
            IUserService userService) : base(options, logger, encoder, clock)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string tempTokenId = null;
            if (Request.HttpContext.WebSockets.IsWebSocketRequest)
                if (!Request.Headers.ContainsKey("token"))
                {
                    // TODO: Пока создаем временного пользователя
                    // return AuthenticateResult.Fail("Unauthorized");

                    var user = await _userService.Upsert(GetTempUser());
                    var token = await _tokenService.IssueToken(user.Id);
                    tempTokenId = token.TokenId;
                }
            
            var tokenId = tempTokenId ?? Request.Headers["token"];
            if (string.IsNullOrEmpty(tokenId))
            {
                return AuthenticateResult.NoResult();
            }
            
            try
            {
                if (!await _authService.ValidateToken(tokenId))
                {
                    return AuthenticateResult.Fail("Unauthorized");
                }

                var user = await _authService.GetUserByToken(tokenId);
                var claims = new List<Claim>()
                {
                    new(ClaimTypes.NameIdentifier, user.Login),
                    new(CustomClaimTypes.Token, tokenId),
                    new(CustomClaimTypes.Login, user.Login),
                    new(CustomClaimTypes.UserId, user.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new System.Security.Principal.GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }
        
        private User GetTempUser()
        {
            return new User()
            {
                Login = Guid.NewGuid().ToString("N"),
                Email = Guid.NewGuid().ToString("N"),
                FirstName = Guid.NewGuid().ToString("N"),
                LastName = Guid.NewGuid().ToString("N"),
                MiddleName = Guid.NewGuid().ToString("N"),
                IsActive = true,
            };
        }
    }
}