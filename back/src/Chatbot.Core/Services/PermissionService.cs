using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Core.Services
{
    public class PermissionService: IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task RefreshPolicy()
        {
            var realPolicies = Helper.GetEnumInfos<SecurityPolicy>().Select(_ => _.Value).OrderBy(_ => _).ToArray();
            var factPolicies = (await _permissionRepository.GetAll()).Select(_ => _.Politic).OrderBy(_ => _).ToArray();
            List<PolicyContainer> containers;
            if (realPolicies.Length > factPolicies.Length)
            {
                containers = realPolicies.Select(_ => new PolicyContainer()
                {
                    Real = _,
                    Fact = factPolicies.FirstOrDefault(f => f == _)
                }).ToList();
            }
            else if (factPolicies.Length > realPolicies.Length)
            {
                containers = factPolicies.Select(_ => new PolicyContainer()
                {
                    Real = realPolicies.FirstOrDefault(f => f == _),
                    Fact = _
                }).ToList();
            }
            else
            {
                containers = new();
                for (int i = 0; i < realPolicies.Length + factPolicies.Length; i++)
                {
                    SecurityPolicy? realPolicy = null;
                    SecurityPolicy? factPolicy = null;
                    if (i < realPolicies.Length)
                    {
                        realPolicy = realPolicies[i];
                        factPolicy = factPolicies.FirstOrDefault(_ => _ == realPolicy);
                    }
                    else
                    {
                        var factPolicyIndex = i - factPolicies.Length;
                        factPolicy = factPolicies[factPolicyIndex];
                        realPolicy = realPolicies.FirstOrDefault(_ => _ == factPolicy);
                    }

                    containers.Add(new PolicyContainer()
                    {
                        Real = realPolicy, 
                        Fact = factPolicy
                    });
                }
            }

            foreach (var container in containers.Where(_ => !_.IsIdentical))
            {
                if (container.NeedAdd)
                {
                    await _permissionRepository.Upsert(new Permission()
                    {
                        Politic = container.Real.Value,
                        Name = container.Real.Value.ToString()
                    });
                    continue;
                }

                if (container.NeedDelete)
                {
                    await _permissionRepository.Delete(container.Fact.Value);
                }
            }
        }

        private class PolicyContainer
        {
            public SecurityPolicy? Real { get; set; }
            public SecurityPolicy? Fact { get; set; }
            
            public bool IsIdentical => Helper.IsEnum(Real.Value) && Helper.IsEnum(Fact.Value);

            public bool NeedAdd => Helper.IsEnum(Real.Value) && !Helper.IsEnum(Fact.Value);
            
            public bool NeedDelete => !Helper.IsEnum(Real.Value) && !Helper.IsEnum(Fact.Value);
        }
    }
}