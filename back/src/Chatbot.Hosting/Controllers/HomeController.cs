using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController: ControllerBase
    {
        [HttpGet]
        public string Hello()
        {
            return "Hello World!!!";
        }
    }
}