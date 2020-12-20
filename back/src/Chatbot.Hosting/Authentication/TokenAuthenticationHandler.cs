using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
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

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IAuthService authService, ITokenService tokenService) : base(options, logger, encoder, clock)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var requestCookie = Request.Cookies["Token"];
            if (!Request.Cookies.ContainsKey("Token"))
                return AuthenticateResult.Fail("Unauthorized");
            
            var tokenId = Request.Cookies["Token"];
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

                if (await _tokenService.IsExpires(tokenId))
                {
                    var token = await _tokenService.Refresh(tokenId);
                    Response.Cookies.Append("Token", "token", new CookieOptions()
                    {
                        Expires = token.DateExpired
                    });
                }

                var user = await _authService.GetUserByToken(tokenId);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Login),
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