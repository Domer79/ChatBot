using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Pipe;

namespace Chatbot.Core.Pipe
{
    public class Pipe: IPipe
    {
        private readonly Func<IPipeContext, Task> _beginFunc;

        public Pipe(PipeConfigurator configurator)
        {
            var handlers = configurator.GetHandlers().ToArray();
            _beginFunc = GetPipeFunc(handlers, 0);
        }

        private Func<IPipeContext, Task> GetPipeFunc(PipeHandler[] handlers, int index)
        {
            if (index == handlers.Length)
            {
                return _ => Task.CompletedTask;
            }

            return (context) =>
            {
                var pipeFunc = GetPipeFunc(handlers, index + 1);
                return handlers[index].InvokeAsync(context, pipeFunc);
            };
        }

        public Task Start(IPipeContext context)
        {
            return _beginFunc(context);
        }
    }
}