using System;

namespace Chatbot.Model.Enums
{
    [Flags]
    public enum DialogStatus
    {
        /// <summary>
        /// Старт диалога, когда пользователь отправляет первое сообщение
        /// </summary>
        Started = 1,
        
        /// <summary>
        /// Оператор берет в работу или первый раз отвечает
        /// </summary>
        Active = 1 << 1,
        
        /// <summary>
        /// Соединение с пользователем прервано, но диалог может быть возобновлен 
        /// </summary>
        Sleep = 1 << 2,
        
        /// <summary>
        /// Диалог закрыт
        /// </summary>
        Closed = 1 << 3,
        
        /// <summary>
        /// Диалог отклонен оператором
        /// </summary>
        Rejected = 1 << 4,
        
        /// <summary>
        /// Офлайн - диалог создан не в рабочее время
        /// </summary>
        /// <remarks>Использовать с осторожностью</remarks>
        Offline = 1 << 5,
    }
}