using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Contracts.Requests
{
    public class DialogPageRequest
    {
        public DialogStatus Status { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public bool? Offline { get; set; }
    }
}