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
        
        OperatorConnect = 7,
        
        #endregion
    }
}