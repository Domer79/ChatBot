using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Pipe;

namespace Chatbot.Core.Pipe
{
    public class Pipe: IPipe
    {
        private readonly IPipeContext _context;
        private readonly Func<Task> _beginFunc;

        public Pipe(IPipeContext context, PipeConfigurator configurator)
        {
            _context = context;
            var handlers = configurator.GetHandlers().ToArray();
            _beginFunc = GetPipeFunc(handlers, 0);
        }

        private Func<Task> GetPipeFunc(PipeHandler[] handlers, int index)
        {
            if (index == handlers.Length)
            {
                return () => Task.CompletedTask;
            }
            
            return () => handlers[index].InvokeAsync(_context, GetPipeFunc(handlers, ++index));
        }

        public Task Start()
        {
            return _beginFunc();
        }
    }
}