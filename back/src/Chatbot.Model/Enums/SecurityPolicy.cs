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
        
        MainPage = 5,
        DialogPage = 6,
        
        #endregion
    }
}