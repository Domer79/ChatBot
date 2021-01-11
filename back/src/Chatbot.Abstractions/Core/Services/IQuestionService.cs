using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IQuestionService
    {
        Task<QuestionResponse> GetQuestion(Guid questionId);

        Task<QuestionResponse[]> GetQuestions(Guid parentId);
        
        Task<QuestionResponse[]> GetAll();
        
        Task<QuestionResponse> Upsert(QuestionResponse question);
        
        Task<bool> Delete(QuestionResponse question);
        Task<QuestionResponse[]> GetAllQuestionsUnlessChild(Guid questionId);
    }
}