using System.Threading.Tasks;
using Chatbot.Abstractions.Pipe;
using Chatbot.Core.Pipe;

namespace Chatbot.Hosting.Hubs.Pipes
{
    public class MessagePipe: Pipe, IMessagePipe
    {
        public MessagePipe(PipeConfigurator configurator) : base(configurator)
        {
        }
    }
}