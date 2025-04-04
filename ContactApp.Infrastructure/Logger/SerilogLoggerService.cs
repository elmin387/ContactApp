using ContactApp.Application.Interfaces.Services;
using Serilog;
using System;

namespace ContactApp.Infrastructure.Logger
{
    public class SerilogLoggerService : ILoggerService
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLoggerService()
        {
            _logger = Log.Logger;
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            if (exception != null)
                _logger.Error(exception, message, args);
            else
                _logger.Error(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warning(message, args);
        }
    }
}