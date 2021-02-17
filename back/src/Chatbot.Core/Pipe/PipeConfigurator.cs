using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Pipe;

namespace Chatbot.Core.Pipe
{
    public class PipeConfigurator
    {
        private readonly List<PipeHandler> _handlers = new();

        public PipeConfigurator RegisterHandler(PipeHandler handler)
        {
            _handlers.Add(handler);
            return this;
        }

        internal PipeHandler[] GetHandlers()
        {
            return _handlers.ToArray();
        }
    }
}