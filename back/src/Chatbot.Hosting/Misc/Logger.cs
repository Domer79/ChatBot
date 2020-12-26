using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Chatbot.Hosting.Misc
{
    public class MyLoggerProvider: ILoggerProvider
    {
        private ILogger _logger;
            
        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            _logger = new Logger(categoryName);
            return _logger;
        }
    }
    
    public class Logger: ILogger
    {
        private readonly NLog.ILogger _logger;

        public Logger(string categoryName)
        {
            _logger = LogManager.GetLogger(categoryName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                {
                    _logger.Log(NLog.LogLevel.Fatal, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Debug:
                {
                    _logger.Log(NLog.LogLevel.Debug, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Error:
                {
                    _logger.Log(NLog.LogLevel.Error, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Information:
                {
                    _logger.Log(NLog.LogLevel.Info, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.None:
                {
                    _logger.Log(NLog.LogLevel.Trace, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Trace:
                {
                    _logger.Log(NLog.LogLevel.Trace, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Warning:
                {
                    _logger.Log(NLog.LogLevel.Warn, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
            }
        }
    }

    public class Logger<T> : ILogger<T>
    {
        private readonly NLog.ILogger _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger(typeof(T).Name);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                {
                    _logger.Log(NLog.LogLevel.Fatal, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Debug:
                {
                    _logger.Log(NLog.LogLevel.Debug, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Error:
                {
                    _logger.Log(NLog.LogLevel.Error, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Information:
                {
                    _logger.Log(NLog.LogLevel.Info, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.None:
                {
                    _logger.Log(NLog.LogLevel.Trace, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Trace:
                {
                    _logger.Log(NLog.LogLevel.Trace, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
                case LogLevel.Warning:
                {
                    _logger.Log(NLog.LogLevel.Warn, CultureInfo.CurrentCulture, state.ToString());
                    break;
                }
            }
        }
    }
}