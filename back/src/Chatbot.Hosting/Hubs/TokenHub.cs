using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public class TokenHub: Hub
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokenHub(
            ITokenService tokenService,
            IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task GetToken()
        {
            var user = await _userService.Upsert(GetTempUser());
            var token = await _tokenService.IssueToken(user.Id);
            await Clients.Caller.SendAsync("setToken", token.TokenId);
        }
        
        private User GetTempUser()
        {
            return new User()
            {
                Login = Guid.NewGuid().ToString("N"),
                Email = Guid.NewGuid().ToString("N"),
                Fio = "Не авторизован",
                Password = "0x0".GetBytes(),
                IsActive = false,
            };
        }
    }
}