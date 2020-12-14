using Microsoft.AspNetCore.Authorization;

namespace Chatbot.Hosting.Authentication
{
    public class PolicyAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Policy { get; }

        public PolicyAuthorizationRequirement(string policy)
        {
            Policy = policy;
        }
    }
}