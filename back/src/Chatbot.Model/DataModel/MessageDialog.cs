using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Chatbot.Model.Enums;

namespace Chatbot.Model.DataModel
{
    public class MessageDialog
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Номер диалога
        /// </summary>
        public int Number { get; set; }
        
        /// <summary>
        /// Дата создания, в UTC
        /// </summary>
        public DateTime DateCreated { get; set; }
        
        /// <summary>
        /// Дата взятия в работу
        /// </summary>
        public DateTime? DateWork { get; set; }
        
        /// <summary>
        /// Дата закрытия или отклонения
        /// </summary>
        public DateTime? DateCompleted { get; set; }
        
        /// <summary>
        /// Статус диалога
        /// </summary>
        public DialogStatus DialogStatus { get; set; }
        
        /// <summary>
        /// Оператор
        /// </summary>
        public Guid? OperatorId { get; set; }
        
        /// <summary>
        /// Клиент
        /// </summary>
        public Guid? ClientId { get; set; }
        
        /// <summary>
        /// Признак того, что диалог был создан в режиме офлайн
        /// </summary>
        public bool Offline { get; set; }
        
        /// <summary>
        /// Идентификатор диалога, на основе которого был создан текущий диалог, после того как предыдущий был закрыт
        /// по истечению времени не активности пользователя 
        /// </summary>
        public Guid? BasedId { get; set; }
        
        public HashSet<Message> Messages { get; set; }
        public User Operator { get; set; }
        public User Client { get; set; }
    }
}