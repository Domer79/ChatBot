using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Contracts
{
    public class DialogUser
    {
        public User User { get; set; }
        public bool IsOperator { get; set; }
        public string ConnectionId { get; set; }
    }
}