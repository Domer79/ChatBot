using System;
using System.Collections.Generic;

namespace Chatbot.Model.DataModel
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public Guid ParentId { get; set; }
        public DateTime DateCreated { get; set; }
        public QuestionResponse Parent { get; set; }
        public HashSet<QuestionResponse> Children { get; set; }
    }
}