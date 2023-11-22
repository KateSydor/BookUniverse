﻿namespace BookUniverse.BLL.Interfaces
{
    public interface ILoggingService
    {
        void LogInformation(string msg);

        void LogWarning(string msg);

        void LogTrace(string msg);

        void LogDebug(string msg);

        void LogError(object request, string errorMsg);
    }
}
