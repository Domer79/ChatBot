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
    }
}