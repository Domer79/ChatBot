using System;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

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

    public class CustomSecurityAttribute : AuthorizeAttribute
    {
        public CustomSecurityAttribute(SecurityPolicy securityPolicy)
        {
            Policy = securityPolicy.ToString();
        }
    }
    
    public class CustomAuthorizationHandler: AuthorizationHandler<PolicyAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyAuthorizationRequirement requirement)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class CustomAuthPolicyProvider: IAuthorizationPolicyProvider
    {
        public CustomAuthPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            try
            {
                //All custom policies created by us will have " : " as delimiter to identify policy name and values
                //Any delimiter or character can be choosen, and it is upto user choice

                var policy = policyName; //Name for policy and values are set in A2AuthorizePermission Attribute

                if (policy!=null)
                {
                    //Dynamically building the AuthorizationPolicy and adding the respective requirement based on the policy names which we define in Authroize Attribute.
                    var policyBuilder = new AuthorizationPolicyBuilder();

                    //Authorize Hanlders are created based on Authroize Requirement type.
                    //Adding the object of A2AuthorizePermissionRequirement will invoke the A2AuthorizePermissionHandler
                    policyBuilder.AddRequirements(new PolicyAuthorizationRequirement(policy));
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

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }
    }
}