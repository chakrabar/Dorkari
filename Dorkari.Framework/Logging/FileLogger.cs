using Dorkari.Framework.Contracts;
using Dorkari.Helpers.Core.Utilities;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Dorkari.Framework.Logging
{
    public class FileLogger : ILogger
    {
        private static LoggingLevel _logLevel;
        
        private LoggingLevel LogLevel
        {
            get
            {
                if (_logLevel == LoggingLevel.UNDEFINED)
                {
                    var logLevel = ConfigurationManager.AppSettings["LogLevel"]; //TODO: to constant
                    var isValidLogLevel = Enum.TryParse<LoggingLevel>((logLevel ?? string.Empty).Trim().ToUpper(), out _logLevel);
                    if (!isValidLogLevel)
                        _logLevel = LoggingLevel.FULL; //if no log level is defined FULL logging
                }
                return LoggingLevel.FULL;
            }
        }

        #region LoggingMethods

        public void LogInfo(string title, string details)
        {
            LogInBackground(LogType.Info, title, details);
        }

        public void LogWarning(string title, string details)
        {
            LogInBackground(LogType.Warning, title, details);
        }

        public void LogError(string title, string details)
        {
            LogInBackground(LogType.Error, title, details);
        }

        public void LogException(string title, string details, Exception ex)
        {
            details += " \n " + ExceptionHelper.GetExceptionAsString(ex);
            LogInBackground(LogType.Exception, title, details);
        }

        #endregion

        #region LoggingExceptionWrapper

        public void LogOnException(string title, string details, System.Action action, bool supressException = false)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!ExceptionHelper.IsAlreadyLogged(ex))
                {
                    LogException(title, details, ex);
                    ExceptionHelper.MarkAsLogged(ex);
                }
                if (!supressException)
                {
                    throw;
                }
            }
        }

        public T LogOnException<T>(string title, string details, Func<T> func, bool supressException = false)
        {
            T returnvalue = default(T);
            LogOnException(title, details, () => { returnvalue = func(); }, supressException);
            return returnvalue;
        }

        #endregion

        #region Private

        private void LogInBackground(LogType type, string title, string details)
        {
            var logLevel = LogLevel;
            if (logLevel == LoggingLevel.NOLOG ||
                    (logLevel == LoggingLevel.ERROR &&
                        (type != LogType.Error && type != LogType.Exception)))
                return;
            //var username 
            //var hostName 
            var utcTime = DateTime.UtcNow;
            Task.Factory.StartNew(() =>
            {
                SaveLogEntity(type, title, details, utcTime, "User", "local");
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.Default)
            .ContinueWith(task =>
            {
                //TODO: log logging failure somewhere else!!
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void SaveLogEntity(LogType type, string title, string details, DateTime utcTime, string user, string hostName)
        {
            var log = new LogEntry
            {
                Details = details,
                Time = utcTime,
                Title = CheckAndAdjustTitle(title),
                Type = type.ToString(),
                User = user,
                HostName = hostName
            };
            FileLogWriter.Add(log);
        }

        private string CheckAndAdjustTitle(string title) //max length taken as 250
        {
            return (string.IsNullOrEmpty(title) || title.Length <= 250)
                ? title : title.Substring(0, 250);
        }

        #endregion
    }
}
