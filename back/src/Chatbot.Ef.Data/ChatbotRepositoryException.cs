using System;

namespace Chatbot.Ef.Data
{
    public class ChatbotRepositoryException : Exception
    {
        public ChatbotRepositoryException(string? message) : base(message)
        {
        }

        public ChatbotRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}