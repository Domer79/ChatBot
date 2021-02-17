using System;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Pipe;
using Chatbot.Model.DataModel;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class MessageContext: IMessagePipeContext
    {
        public MessageContext(Message message, User user, ChatHub chatHub)
        {
            Message = message;
            User = user;
            ChatHub = chatHub;
        }

        public Message Message { get; set; }
        public DialogGroup DialogGroup { get; set; }
        public User User { get; }
        public ChatHub ChatHub { get; }
    }
}