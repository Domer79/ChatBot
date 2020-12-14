using Chatbot.Abstractions;
using Chatbot.Hosting.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController: ControllerBase
    {
        [CustomSecurity(SecurityPolicy.ReadMessage)]
        [HttpGet]
        public string Hello()
        {
            return "Hello World!!!";
        }

        [HttpGet]
        public string Hello2()
        {
            return "Hello2 World!!!";
        }
    }
}