using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class RoleService: IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public Task<Permission[]> GetPermissions(Guid[] roleIds)
        {
            return _roleRepository.GetPermissions(roleIds);
        }

        public Task<Role[]> GetRoles()
        {
            return _roleRepository.GetAll();
        }

        public async Task<bool> SetRole(Guid roleId, Guid userId, bool set)
        {
            if (!set) 
                return await _userRoleRepository.Delete(userId, roleId);
            
            await _userRoleRepository.Add(userId, roleId);
            return true;
        }
    }
}