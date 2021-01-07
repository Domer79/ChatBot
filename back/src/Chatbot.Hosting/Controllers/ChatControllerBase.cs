using System;
using System.Threading.Tasks;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    public class ChatControllerBase: ControllerBase
    {
        private User _user;

        protected string GetToken()
        {
            if (!Request.Headers.ContainsKey("token"))
                return null;

            return Request.Headers["token"];
        }

        protected string Login => HttpContext.User.GetLogin();
        protected Guid? UserId => HttpContext.User.GetUserId();
    }
}