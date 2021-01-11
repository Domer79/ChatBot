using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class QuestionRepository: IQuestionRepository
    {
        private readonly ChatbotContext _context;

        public QuestionRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<QuestionResponse> GetQuestion(Guid questionId)
        {
            return _context.Questions.SingleOrDefaultAsync(_ => _.Id == questionId);
        }

        public Task<QuestionResponse[]> GetAll()
        {
            return _context.Questions.OrderBy(_ => _.Number).ToArrayAsync();
        }

        public async Task<QuestionResponse> Upsert(QuestionResponse question)
        {
            if (question.Id == Guid.Empty)
            {
                question.Id = Guid.NewGuid();
                _context.Questions.Add(question);
            }
            else
            {
                _context.Update(question).Property(_ => _.Number).IsModified = false;
            }

            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> Delete(QuestionResponse question)
        {
            _context.Remove(question);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<QuestionResponse[]> GetQuestionChildren(Guid parentId)
        {
            return parentId == Guid.Empty
                ? _context.Questions.Where(_ => _.ParentId == null).OrderBy(_ => _.Number).ToArrayAsync()
                : _context.Questions.Where(_ => _.ParentId == parentId).OrderBy(_ => _.Number).ToArrayAsync();
        }

        public Task<QuestionResponse[]> GetAllQuestionsUnlessChild(Guid questionId)
        {
            return _context.Questions
                .Where(_ => _.ParentId != questionId && _.Id != questionId)
                .OrderBy(_ => _.Number)
                .ToArrayAsync();
        }
    }
}