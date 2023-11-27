﻿namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.Interfaces;
    using Serilog;

    public class LoggingService : ILoggingService
    {
        private readonly ILogger _logger;

        public LoggingService(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string msg)
        {
            _logger.Information($"{msg}");
        }

        public void LogWarning(string msg)
        {
            _logger.Warning($"{msg}");
        }

        public void LogTrace(string msg)
        {
            _logger.Information($"{msg}");
        }

        public void LogDebug(string msg)
        {
            _logger.Debug($"{msg}");
        }

        public void LogError(object? request, string errorMsg)
        {
            if (request != null)
            {
                string requestType = request.GetType().ToString();
                string requestClass = requestType.Substring(requestType.LastIndexOf('.') + 1);
                _logger.Error($"{requestClass} handled with the error: {errorMsg}");
            }
            else
            {
                _logger.Error(errorMsg);
            }
        }
    }
}
