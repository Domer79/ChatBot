using System;
using Chatbot.Common.Abstracts;
using Microsoft.Extensions.Configuration;

namespace Chatbot.Common
{
    public class AppConfig: IAppConfig
    {
        private readonly IConfiguration _configuration;

        // public AppConfig(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        // }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("chatbot");
        }
    }
}