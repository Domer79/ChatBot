using System;
using System.Runtime.Serialization;

namespace Chatbot.Core.Exceptions
{
    public class ChatbotCoreException: Exception
    {
        public ChatbotCoreException(): this(ErrorType.None)
        {
        }

        protected ChatbotCoreException(SerializationInfo info, StreamingContext context) 
            : this(info, context, ErrorType.None)
        {
        }

        public ChatbotCoreException(string message) : this(message, ErrorType.None)
        {
        }

        public ChatbotCoreException(string message, Exception innerException) 
            : this(message, innerException, ErrorType.None)
        {
        }
        
        public ChatbotCoreException(ErrorType errorType)
        {
            ErrorType = errorType;
        }

        protected ChatbotCoreException(SerializationInfo info, StreamingContext context, ErrorType errorType) : base(info, context)
        {
            ErrorType = errorType;
        }

        public ChatbotCoreException(string message, ErrorType errorType) : base(message)
        {
            ErrorType = errorType;
        }

        public ChatbotCoreException(string message, Exception innerException, ErrorType errorType) : base(message, innerException)
        {
            ErrorType = errorType;
        }

        public ErrorType ErrorType { get; }
    }
}