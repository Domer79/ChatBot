using Chatbot.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Chatbot.Hosting.Authentication
{
    public class CustomSecurityAttribute : AuthorizeAttribute
    {
        public CustomSecurityAttribute(SecurityPolicy securityPolicy)
        {
            Policy = securityPolicy.ToString();
            AuthenticationSchemes = "Token";
        }
    }
}