using System;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Abstractions.Pipe;
using Chatbot.Model.DataModel;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public interface IMessagePipeContext: IPipeContext
    {
        Message Message { get; set; }
        IDialogGroup DialogGroup { get; set; }
        User User { get; }
        ChatHub ChatHub { get; }
        Boolean IsNewDialog { get; }
    }
}