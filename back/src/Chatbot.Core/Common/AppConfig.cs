using System;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Model.Configuration;
using Chatbot.Model.DataModel;
using Microsoft.Extensions.Configuration;

namespace Chatbot.Core.Common
{
    public class AppConfig: IAppConfig
    {
        private readonly IConfiguration _configuration;
        private ChatConfig _chatConfig;

        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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

        public ChatConfig Chat => _chatConfig ??= _configuration.GetSection("Chat").Get<ChatConfig>();
    }
}