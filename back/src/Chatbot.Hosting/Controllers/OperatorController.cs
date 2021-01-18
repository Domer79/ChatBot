using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Requests;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [CustomSecurity(SecurityPolicy.OperatorManager)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OperatorController: ChatControllerBase
    {
        private readonly IUserService _userService;
        private readonly Mapper _mapper;

        public OperatorController(
            IUserService userService,
            Mapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Page<UserResponse>> GetPage([FromQuery] UserPageRequest request)
        {
            var page = await _userService.GetPage(request.Number, request.Size, request.IsActive);

            return new Page<UserResponse>()
            {
                Items = _mapper.Map<UserResponse[]>(page.Items),
                TotalCount = page.TotalCount
            };
        }

        [HttpPost]
        public async Task<UserResponse> Upsert([FromBody] User userRequest)
        {
            var user = await _userService.Upsert(userRequest);
            return _mapper.Map<UserResponse>(user);
        }

        [HttpPut]
        public async Task Block(Guid userId)
        {
            var user = await _userService.GetById(userId);
            user.IsActive = false;
            await _userService.Upsert(user);
        }

        [HttpPut]
        public async Task Activate(Guid userId)
        {
            var user = await _userService.GetById(userId);
            user.IsActive = true;
            await _userService.Upsert(user);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid userId)
        {
            return await _userService.Remove(userId);
        }
    }
}