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
        private readonly List<Type> _handlerServices = new();

        public PipeConfigurator AddHandler(PipeHandler handler)
        {
            _handlers.Add(handler);
            return this;
        }

        public PipeConfigurator AddHandlerService<TPipeHandler>() where TPipeHandler: PipeHandler
        {
            _handlerServices.Add(typeof(TPipeHandler));
            return this;
        }

        internal PipeHandler[] GetHandlers()
        {
            return _handlers.ToArray();
        }

        public Type[] GetHandlerServices()
        {
            return _handlerServices.ToArray();
        }
    }
}