using Microsoft.AspNetCore.Authentication;

namespace Chatbot.Hosting.Authentication
{
    public class TokenAuthenticationOptions: AuthenticationSchemeOptions
    {
        public TokenAuthenticationOptions()
        {
        }

        public const string SchemeName = "Token";
    }
}