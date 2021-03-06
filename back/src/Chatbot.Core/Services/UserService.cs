﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Core.Exceptions;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class UserService: IUserService
    {
        private const string PasswordSalt = "D7107E9FB4424E85A27A9F5A0F972204";
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User[]> GetAll()
        {
            return _userRepository.GetAll();
        }

        public async Task<Page<User>> GetPage(int pageNumber, int pageSize, bool? isActive)
        {
            User[] users = await _userRepository.GetPage(pageNumber, pageSize, isActive);
            long totalCount = await _userRepository.GetTotalCount(isActive);

            return new Page<User>()
            {
                Items = users,
                TotalCount = totalCount
            };
        }

        public Task<User> GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public Task<User[]> GetByIds(Guid[] ids)
        {
            return _userRepository.GetByIds(ids);
        }

        public Task<User> GetByLogin(string login)
        {
            return _userRepository.GetByLogin(login);
        }

        public Task<User> Upsert(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userRepository.Upsert(user);
        }

        public async Task<bool> Remove(Guid userId)
        {
            var user = await GetById(userId);
            if (user == null)
                throw new ChatbotCoreException($"User by user id {userId} not found", ErrorType.UserNotFound);
            
            return await _userRepository.Remove(user);
        }

        public Task Remove(User user)
        {
            return _userRepository.Remove(user);
        }

        public async Task<bool> ValidatePassword(Guid userId, string password)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
                throw new ChatbotCoreException($"User by user id {userId} not found", ErrorType.UserNotFound);
            return ValidatePassword(user, password);
        }

        public async Task<bool> ValidatePassword(string login, string password)
        {
            var user = await GetByLogin(login);
            if (user == null)
                throw new ChatbotCoreException($"User by login {login} not found", ErrorType.UserNotFound);
            return ValidatePassword(user, password);
        }

        public bool ValidatePassword(User user, string password)
        {
            if (user == null || !user.IsActive)
                return false;

            try
            {
                var hashPassword = password.GetSHA1HashBytes();
                hashPassword = hashPassword.Concat(PasswordSalt.GetSHA1HashBytes()).ToArray().GetSHA1HashBytes();
                var hashPassword2 = user.Password;
                return hashPassword.SequenceEqual(hashPassword2);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SetPassword(Guid userId, string password)
        {
            var user = await GetById(userId);
            return await SetPassword(user, password);
        }

        public async Task<bool> SetPassword(string login, string password)
        {
            var user = await GetByLogin(login);
            return await SetPassword(user, password);
        }

        public async Task<bool> SetPassword(User user, string password)
        {
            if (user == null)
                throw new ChatbotCoreException($"User not found", ErrorType.UserNotFound);

            try
            {
                var hashPassword = password.GetSHA1HashBytes();
                hashPassword = hashPassword.Concat(PasswordSalt.GetSHA1HashBytes()).ToArray().GetSHA1HashBytes();
                user.Password = hashPassword;
                await _userRepository.SetPassword(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<Role[]> GetRoles(Guid userId)
        {
            return _userRepository.GetRoles(userId);
        }

        public Task<Role[]> GetRoles(string login)
        {
            return _userRepository.GetRoles(login);
        }
    }
}