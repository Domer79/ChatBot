﻿using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class QuestionService: IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public Task<QuestionResponse> GetQuestion(Guid questionId)
        {
            return _questionRepository.GetQuestion(questionId);
        }

        public Task<QuestionResponse[]> GetAll()
        {
            return _questionRepository.GetAll();
        }

        public Task<QuestionResponse> Upsert(QuestionResponse question)
        {
            return _questionRepository.Upsert(question);
        }

        public Task<bool> Delete(QuestionResponse question)
        {
            return _questionRepository.Delete(question);
        }
    }
}