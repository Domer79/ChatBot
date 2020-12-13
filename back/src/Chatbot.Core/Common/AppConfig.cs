using System;
using Chatbot.Abstractions;
using Chatbot.Model.Configuration;
using Chatbot.Model.DataModel;
using Microsoft.Extensions.Configuration;

namespace Chatbot.Core.Common
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

        public int GetTokenLifetime()
        {
            return _configuration.GetSection("Token").Get<TokenConfiguration>().Lifetime;
        }

        public int GetTokenAutoExpired()
        {
            return _configuration.GetSection("Token").Get<TokenConfiguration>().AutoExpired;
        }
    }
}