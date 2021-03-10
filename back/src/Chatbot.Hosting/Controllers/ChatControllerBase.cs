using System;
using Chatbot.Common;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    public class ChatControllerBase: ControllerBase
    {
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