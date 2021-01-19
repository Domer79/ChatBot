namespace Chatbot.Abstractions.Contracts.Requests
{
    public class UserPageRequest
    {
        public int Number { get; set; }
        public int Size { get; set; }
        public bool? IsActive { get; set; }
    }
}