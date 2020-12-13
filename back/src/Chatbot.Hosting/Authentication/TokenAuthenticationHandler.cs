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
        private readonly IUserService _userService;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            // _authService = authService;
            // _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "token"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return await Task.FromResult(AuthenticateResult.Success(ticket));
            //
            //
            // if (!Request.Headers.ContainsKey("Token"))
            //     return AuthenticateResult.Fail("Unauthorized");
            //
            // string tokenId = Request.Headers["Token"];
            // if (string.IsNullOrEmpty(tokenId))
            // {
            //     return AuthenticateResult.NoResult();
            // }
            //
            // try
            // {
            //     if (!await _authService.ValidateToken(tokenId))
            //     {
            //         return AuthenticateResult.Fail("Unauthorized");
            //     }
            //     
            //     var claims = new List<Claim>()
            //     {
            //         new Claim(ClaimTypes.Name, tokenId),
            //     };
            //     var identity = new ClaimsIdentity(claims, Scheme.Name);
            //     var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            //     var ticket = new AuthenticationTicket(principal, Scheme.Name);
            //     return AuthenticateResult.Success(ticket);
            // }
            // catch (Exception ex)
            // {
            //     return AuthenticateResult.Fail(ex.Message);
            // }
        }
    }
}