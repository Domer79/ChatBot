using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [AllowAnonymous]
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

        [HttpGet]
        public async Task<QuestionResponseObject> GetQuestion(Guid questionId)
        {
            var question = await _questionService.GetQuestion(questionId);
            return _mapper.Map<QuestionResponseObject>(question);
        }

        [HttpGet]
        public async Task<QuestionResponseObject[]> GetQuestions(Guid parentId)
        {
            var questions = await _questionService.GetQuestions(parentId);
            return _mapper.Map<QuestionResponseObject[]>(questions);
        }

        [HttpGet]
        public async Task<bool> ExistQuestions(Guid parentId)
        {
            var questions = await GetQuestions(parentId);
            return questions.Length > 0;
        }
    }
}