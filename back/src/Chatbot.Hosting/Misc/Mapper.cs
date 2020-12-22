﻿using AutoMapper;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Model.DataModel;

namespace Chatbot.Hosting.Misc
{
    public class Mapper
    {
        private IMapper _mapper;

        public Mapper()
        {
            _mapper = Configure();
        }

        private IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponse>();
                cfg.CreateMap<Token, TokenResponse>();
            });

            return config.CreateMapper();
        }

        public TDest Map<TDest>(object source)
        {
            return _mapper.Map<TDest>(source);
        }
    }
}