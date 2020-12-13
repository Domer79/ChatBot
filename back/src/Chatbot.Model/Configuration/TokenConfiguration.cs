namespace Chatbot.Model.Configuration
{
    public class TokenConfiguration
    {
        /// <summary>
        /// Время жизни токена, в часах
        /// </summary>
        public int Lifetime { get; set; }
        
        /// <summary>
        /// Время в минутах, до истечения времени жизни токена, пока его можно еще обновить
        /// </summary>
        public int AutoExpired { get; set; }
    }
}