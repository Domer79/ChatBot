namespace Chatbot.Abstractions.Contracts.Requests
{
    public class LoginRequest
    {
        public string LoginOrEmail { get; set; }
        public string Password { get; set; }
    }
}