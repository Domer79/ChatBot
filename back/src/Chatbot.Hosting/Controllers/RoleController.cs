using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Requests;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Core.Common;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Misc;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController: ChatControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly Mapper _mapper;

        public RoleController(
            IRoleService roleService,
            IUserRoleService userRoleService,
            Mapper mapper)
        {
            _roleService = roleService;
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [CustomSecurity(SecurityPolicy.OperatorManager)]
        [HttpGet]
        public async Task<UserRoleResponse[]> GetRoles(Guid? userId)
        {
            var roles = await _roleService.GetRoles();
            var response = _mapper.Map<UserRoleResponse[]>(roles);
            if (!userId.HasValue)
                return response;
            
            foreach (var role in response)
            {
                role.Setted = await _userRoleService.Check(userId.Value, role.Id);
            }

            return response;
        }

        [CustomSecurity(SecurityPolicy.OperatorManager)]
        [HttpPut]
        public async Task<bool> SetRole([FromBody] SetRoleRequest request)
        {
            var result = await _roleService.SetRole(request.RoleId, request.UserId, request.Set);
            return result;
        }
    }
}