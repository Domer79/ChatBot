using System;

namespace Chatbot.Abstractions.Contracts.Chat
{
    public class ChatConfig
    {
        public int DecayMinutes { get; set; }
        public TimeSpan DecayTime => TimeSpan.FromMinutes(DecayMinutes);

    }
}