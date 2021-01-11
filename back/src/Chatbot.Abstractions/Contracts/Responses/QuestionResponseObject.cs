using System;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class QuestionResponseObject
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}