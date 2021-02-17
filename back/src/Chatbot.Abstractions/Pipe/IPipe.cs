using System;
using System.Threading.Tasks;

namespace Chatbot.Abstractions.Pipe
{
    public interface IPipe
    {
        Task Start(IPipeContext context);
    }
}