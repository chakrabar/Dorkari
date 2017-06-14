using System;

namespace Dorkari.Framework.Contracts
{
    public interface ILogger
    {
        void LogInfo(string title, string details);
        void LogWarning(string title, string details);
        void LogError(string title, string details);
        void LogException(string title, string details, Exception ex);
        void LogOnException(string title, string details, Action action, bool supressException = false);
        T LogOnException<T>(string title, string details, Func<T> func, bool supressException = false);
    }
}
