using Chatbot.Abstractions;
using Chatbot.Hosting.Authentication;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController: ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [CustomSecurity(SecurityPolicy.ReadMessage)]
        [HttpGet]
        public string Hello()
        {
            _logger.LogInformation("Hello World!!!");
            return "Hello World!!!";
        }

        [HttpGet]
        public string Hello2()
        {
            return "Hello2 World!!!";
        }
    }
}