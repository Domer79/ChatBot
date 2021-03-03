using System.ComponentModel;

namespace Chatbot.Model.Enums
{
    public enum SecurityPolicy
    {
        [Description("Чтение сообщений")]
        ReadMessage = 1,
        
        [Description("Добавление пользователя")]
        AddUser = 2,
        
        [Description("Изменение пользователя")]
        ChangeUser = 3,
        
        [Description("Удаление пользователя")]
        RemoveUser = 4,
        
        #region Front pages
        
        [Description("Главная страница")]
        MainPage = 5,
        
        [Description("Страница диалогов")]
        DialogPage = 6,
        
        #endregion

        #region Действия операторов
        
        [Description("Подключение оператора")]
        OperatorConnection = 7,

        [Description("Управление вопросами")]
        QuestionManager = 8,
        
        [Description("Управление операторами")]
        OperatorManager = 9,

        [Description("Управление настройками")]
        SettingsManager = 10,
        
        #endregion
        
        [Description("Разработчик")]
        Developer = 11,
        
        #region Front Dialogs Menu
        
        /// <summary>
        /// Все диалоги
        /// </summary>
        DialogsAll = 12,
        
        /// <summary>
        /// Открытые диалоги
        /// </summary>
        DialogsOpened = 13,
        
        /// <summary>
        /// Отклоненные диалоги
        /// </summary>
        DialogsRejected = 14,
        
        /// <summary>
        /// Диалоги в работе
        /// </summary>
        DialogsActivated = 15,
        
        /// <summary>
        /// Закрытые диалоги
        /// </summary>
        DialogsClosed = 16,
        
        /// <summary>
        /// Диалоги офлайн
        /// </summary>
        DialogsOffline = 17,

        #endregion
    }
}