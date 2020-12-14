using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chatbot.Hosting.Authentication
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        private readonly IAuthService _authService;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IAuthService authService) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Token"))
                return AuthenticateResult.Fail("Unauthorized");
            
            string tokenId = Request.Headers["Token"];
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
                    new Claim(CustomClaimTypes.Token, tokenId),
                    new Claim(CustomClaimTypes.Login, user.Login),
                    new Claim(CustomClaimTypes.UserId, user.Id.ToString())
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
    }
}