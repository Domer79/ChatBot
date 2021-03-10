using System;
using System.ComponentModel;

namespace Chatbot.Model.Enums
{
    [Flags]
    public enum DialogStatus
    {
        /// <summary>
        /// Старт диалога, когда пользователь отправляет первое сообщение
        /// </summary>
        [Description("Открыт")]
        Started = 1,
        
        /// <summary>
        /// Оператор берет в работу или первый раз отвечает
        /// </summary>
        [Description("В работе")]
        Active = 1 << 1,
        
        /// <summary>
        /// Соединение с пользователем прервано, но диалог может быть возобновлен 
        /// </summary>
        [Obsolete]
        [Description("Спит (Не используется)")]
        Sleep = 1 << 2,
        
        /// <summary>
        /// Диалог закрыт
        /// </summary>
        [Description("Закрыт")]
        Closed = 1 << 3,
        
        /// <summary>
        /// Диалог отклонен оператором
        /// </summary>
        [Description("Отклонен")]
        Rejected = 1 << 4,
        
        /// <summary>
        /// Офлайн - диалог создан не в рабочее время
        /// </summary>
        /// <remarks>Использовать с осторожностью</remarks>
        [Description("Офлайн")]
        Offline = 1 << 5,
        
        /// <summary>
        /// Закрыт, отклонен, или офлайн
        /// </summary>
        NotActive = Closed | Rejected | Offline,
    }
}