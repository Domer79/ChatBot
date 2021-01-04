using System;
using Chatbot.Abstractions;

namespace Chatbot.Hosting.Hubs
{
    public static class ChatConfig
    {
        private static IAppConfig _config;

        public static void SetConfig(IAppConfig config)
        {
            _config = config;
        }

        public static TimeSpan DecayTime => _config.Chat.DecayTime;
    }
}