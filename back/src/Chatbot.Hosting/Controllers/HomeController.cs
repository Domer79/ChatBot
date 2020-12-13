﻿using Chatbot.Abstractions;
using Chatbot.Hosting.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController: ControllerBase
    {
        // [CustomSecurity(SecurityPolicy.ReadMessage)]
        [Authorize(Policy = "ReadMessage")]
        [HttpGet]
        public string Hello()
        {
            return "Hello World!!!";
        }
    }
}