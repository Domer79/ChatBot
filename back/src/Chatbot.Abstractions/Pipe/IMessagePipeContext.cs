using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Pipe
{
    public interface IMessagePipeContext
    {
        Message Message { get; }
    }
}