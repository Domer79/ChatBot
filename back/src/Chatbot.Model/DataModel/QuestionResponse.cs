using System;
using System.Collections.Generic;

namespace Chatbot.Model.DataModel
{
    public class QuestionResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Глобальный номер вопроса
        /// </summary>
        public int Number { get; set; }
        
        /// <summary>
        /// Вопрос
        /// </summary>
        public string Question { get; set; }
        
        /// <summary>
        /// Ответ
        /// </summary>
        public string Response { get; set; }
        
        /// <summary>
        /// Ролительский вопрос
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreated { get; set; }
        
        /// <summary>
        /// Номер по порядку среди одного уровня
        /// </summary>
        public int SortOrder { get; set; }
        
        public QuestionResponse Parent { get; set; }
        public HashSet<QuestionResponse> Children { get; set; }
    }
}