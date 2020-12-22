using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public abstract class HubBase: Hub
    {
        private readonly IUserService _userService;

        protected HubBase(IUserService userService)
        {
            _userService = userService;
        }

        protected Task<User> GetUser()
        {
            var login = Context.User.GetLogin();
            return _userService.GetByLoginOrEmail(login);
        }
    }
}