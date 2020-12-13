using System;

namespace Chatbot.Abstractions
{
    public interface IAppConfig
    {
        string GetConnectionString();
        int GetTokenLifetime();
        int GetTokenAutoExpired();
    }
}