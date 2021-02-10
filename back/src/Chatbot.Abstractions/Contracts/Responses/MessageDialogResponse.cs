using System;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Contracts.Responses
{
    public class MessageDialogResponse
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateWork { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DialogStatus DialogStatus { get; set; } 
        public Guid? OperatorId { get; set; }
        public Guid? ClientId { get; set; }
        public bool Offline { get; set; }
        public string FirstMessage { get; set; }
        public UserResponse Client { get; set; }
        public UserResponse Operator { get; set; }
    }
}