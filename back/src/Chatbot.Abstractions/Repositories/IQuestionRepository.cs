using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface IQuestionRepository
    {
        Task<QuestionResponse> GetQuestion(Guid questionId);
        Task<QuestionResponse[]> GetAll();
        
        Task<QuestionResponse> Upsert(QuestionResponse question);
        Task<bool> Delete(QuestionResponse question);
    }
}