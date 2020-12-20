namespace Chatbot.Model.Enums
{
    public enum DialogStatus
    {
        /// <summary>
        /// Старт диалога, когда пользователь отправляет первое сообщение
        /// </summary>
        Started = 1,
        
        /// <summary>
        /// Оператор берет в работу или первый раз отвечает
        /// </summary>
        Active = 2,
        
        /// <summary>
        /// Соединение с пользователем прервано, но диалог может быть возобновлен 
        /// </summary>
        Sleep = 3,
        
        /// <summary>
        /// Диалог закрыт
        /// </summary>
        Closed = 4,
        
        /// <summary>
        /// Диалог отклонен оператором
        /// </summary>
        Rejected = 5,
    }
}