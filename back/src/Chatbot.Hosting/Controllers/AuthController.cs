using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Requests;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Core.Common;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController: ChatControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly Mapper _mapper;

        public AuthController(
            IAuthService authService, 
            IUserService userService, 
            Mapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<bool> CheckAccess(string policy)
        {
            var tokenId = GetToken();
            if (tokenId == null)
                return false;

            if (!await _authService.ValidateToken(tokenId))
                return false;
            
            var securityPolicy = Enum.Parse<SecurityPolicy>(policy);
            return await _authService.CheckAccessByToken(securityPolicy, tokenId);
        }

        [HttpPost]
        public async Task<TokenResponse> Login(LoginRequest request)
        {
            var token = await _authService.LogIn(request.LoginOrEmail, request.Password);
            return token == null ? null : _mapper.Map<TokenResponse>(token);
        }

        [HttpPost]
        public async Task<bool> SetPassword(PasswordRequest request)
        {
            return await _userService.SetPassword(request.UserId, request.Password);
        }
    }
}