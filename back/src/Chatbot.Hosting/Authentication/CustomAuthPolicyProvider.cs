using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Chatbot.Hosting.Authentication
{
    public class CustomAuthPolicyProvider: IAuthorizationPolicyProvider
    {
        public CustomAuthPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            DefaultPolicy = options.Value.DefaultPolicy;
        }

        public AuthorizationPolicy DefaultPolicy { get; set; }

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            try
            {
                var policy = policyName;

                if (policy != null)
                {
                    var policyBuilder = new AuthorizationPolicyBuilder()
                        .AddRequirements(new PolicyAuthorizationRequirement(policy))
                        .AddRequirements(DefaultPolicy.Requirements.ToArray())
                        .AddAuthenticationSchemes(DefaultPolicy.AuthenticationSchemes.ToArray());
                    return Task.FromResult(policyBuilder.Build());
                }
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
            catch(Exception ex)
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }
    }
}