namespace Chatbot.Abstractions.Contracts.Requests
{
    public class UserPageRequest
    {
        public bool? IsActive { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
    }
}