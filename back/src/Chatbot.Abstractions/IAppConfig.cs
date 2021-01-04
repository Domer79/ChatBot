using System;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Chat;

namespace Chatbot.Abstractions
{
    public interface IAppConfig
    {
        string GetConnectionString();
        int GetTokenLifetime();
        int GetTokenAutoExpired();
        
        ChatConfig Chat { get; }
    }
}