using System;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Pipe;
using Chatbot.Model.DataModel;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class MessageContext: IMessagePipeContext
    {
        public MessageContext(Message message, User user, ChatHub chatHub, bool isNewDialog)
        {
            Message = message;
            User = user;
            ChatHub = chatHub;
            IsNewDialog = isNewDialog;
        }

        public Message Message { get; set; }
        public IDialogGroup DialogGroup { get; set; }
        public User User { get; }
        public ChatHub ChatHub { get; }
        public bool IsNewDialog { get; }
    }
}