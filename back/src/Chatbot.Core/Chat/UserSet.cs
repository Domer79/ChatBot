using System;
using System.Collections.Generic;
using System.Linq;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Contracts.Chat;

namespace Chatbot.Core.Chat
{
    public class UserSet
    {
        private readonly IAppConfig _appConfig;
        private readonly Dictionary<Guid, UserConnection> _userConnections = new();
        private static readonly object Lock = new();

        public UserSet(IAppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        /// <summary>
        /// Возвращает состояние подключения пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public UserConnection this[Guid userId]
        {
            get
            {
                lock (Lock)
                {
                    if (!_userConnections.TryGetValue(userId, out var connection))
                    {
                        connection = new UserConnection()
                        {
                            UserId = userId,
                            LastActivity = DateTime.Now
                        };
                        _userConnections.Add(userId, connection);
                    }

                    return _userConnections[userId];
                }
            }
        }

        public void Active(Guid userId)
        {
            lock (Lock)
            {
                if (!_userConnections.ContainsKey(userId))
                    throw new KeyNotFoundException($"User Id {userId} not found");

                var connection = _userConnections[userId];
                connection.RefreshActivity();
            }
        }

        public void SetConnectionId(Guid userId, string connectionId)
        {
            lock (Lock)
            {
                if (!_userConnections.TryGetValue(userId, out var connection))
                {
                    connection = new()
                    {
                        UserId = userId,
                    };

                    _userConnections[userId] = connection;
                }

                connection.ConnectionId = connectionId;
                connection.RefreshActivity();
            }
        }

        public void RemoveInactiveUsers()
        {
            lock (Lock)
            {
                var keys = _userConnections.Keys;
                foreach (var key in keys.Where(key => _userConnections[key].CheckIsNotActivity(_appConfig.Chat.DecayTime)))
                {
                    _userConnections.Remove(key);
                }
            }
        }
    }
}