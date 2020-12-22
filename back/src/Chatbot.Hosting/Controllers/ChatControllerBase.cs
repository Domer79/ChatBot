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
    }
}