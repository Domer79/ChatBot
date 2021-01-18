using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;

namespace Chatbot.Core.Services
{
    public class UserRoleService: IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public Task Add(Guid userId, Guid roleId)
        {
            return _userRoleRepository.Add(userId, roleId);
        }

        public Task<bool> Delete(Guid userId, Guid roleId)
        {
            return _userRoleRepository.Delete(userId, roleId);
        }

        public Task<bool> Check(Guid userId, Guid roleId)
        {
            return _userRoleRepository.Check(userId, roleId);
        }
    }
}