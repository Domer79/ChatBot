using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController: ChatControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly Mapper _mapper;

        public QuestionController(
            IQuestionService questionService,
            Mapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<QuestionResponseObject> GetQuestion(Guid questionId)
        {
            var question = await _questionService.GetQuestion(questionId);
            return _mapper.Map<QuestionResponseObject>(question);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<QuestionResponseObject[]> GetQuestions(Guid parentId)
        {
            var questions = await _questionService.GetQuestions(parentId);
            return _mapper.Map<QuestionResponseObject[]>(questions);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> ExistQuestions(Guid parentId)
        {
            var questions = await GetQuestions(parentId);
            return questions.Length > 0;
        }

        [CustomSecurity(SecurityPolicy.OperationsWithQuestions)]
        [HttpGet]
        public async Task<QuestionResponseObject[]> GetAllQuestionsUnlessChild(Guid? questionId)
        {
            QuestionResponse[] questions;
            if (questionId.HasValue)
                questions = await _questionService.GetAllQuestionsUnlessChild(questionId.Value);
            else
            {
                questions = await _questionService.GetAll();
            }
            
            return _mapper.Map<QuestionResponseObject[]>(questions);
        }

        [CustomSecurity(SecurityPolicy.OperationsWithQuestions)]
        [HttpPost]
        public async Task Save(QuestionResponse question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            await _questionService.Upsert(question);
        }

        [CustomSecurity(SecurityPolicy.OperationsWithQuestions)]
        [HttpDelete]
        public async Task Delete(Guid questionId)
        {
            if (questionId == Guid.Empty) throw new ArgumentException(nameof(questionId));
            var question = await _questionService.GetQuestion(questionId);
            await _questionService.Delete(question);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<QuestionResponseObject[]> GetAll()
        {
            var questions = await _questionService.GetAll();
            return _mapper.Map<QuestionResponseObject[]>(questions);
        }
    }
}