﻿using AutoMapper;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Common
{
    public class Mapper
    {
        private readonly IMapper _mapper;

        public Mapper()
        {
            _mapper = Configure();
        }

        private static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponse>();
                cfg.CreateMap<Token, TokenResponse>();
                cfg.CreateMap<MessageDialog, MessageDialogResponse>();
                cfg.CreateMap<Message, MessageResponse>();
                cfg.CreateMap<QuestionResponse, QuestionResponseObject>();
                cfg.CreateMap<Role, RoleResponse>();
                cfg.CreateMap<Role, UserRoleResponse>();
            });

            return config.CreateMapper();
        }

        public TDest Map<TDest>(object source)
        {
            return _mapper.Map<TDest>(source);
        }
    }
}