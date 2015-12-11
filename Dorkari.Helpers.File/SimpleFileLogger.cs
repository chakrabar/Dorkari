using System;
using System.Configuration;
using System.IO;

namespace Dorkari.Helpers.Files
{
    public static class SimpleFileLogger
    {
        static readonly object _syncObject = new object();
        static readonly string LogPath;
        static readonly bool IsDailyLogginOn;

        static SimpleFileLogger ()
	    {
            var appSettingsLogPath = ConfigurationManager.AppSettings[Constants.LogFilePathKey];
            if (!string.IsNullOrEmpty(appSettingsLogPath) && Directory.Exists(appSettingsLogPath))
                LogPath = appSettingsLogPath;
            else
                LogPath = FileHelper.GetUserDirectory();

            var appSettingsIsDailyLogging = ConfigurationManager.AppSettings[Constants.DailyLogKey];
            bool isDailyLogging = false;
            if (!string.IsNullOrEmpty(appSettingsIsDailyLogging) && bool.TryParse(appSettingsIsDailyLogging, out isDailyLogging))
                IsDailyLogginOn = isDailyLogging;
	    }

        public static void LogMessage(string message)
        {
            Log(message, "Message");
        }

        public static void LogWarning(string message)
        {
            Log(message, "Warning");
        }

        public static void LogError(string message)
        {
            Log(message, "ERROR");
        }

        public static void LogException(string message, Exception ex) //TODO: inner exceptions
        {
            var exceptionDetails = message + ". Exception: " + ex.Message + ". Stack Trace: " + ex.StackTrace;
            Log(exceptionDetails, "EXCEPTION");
        }

        static void Log(string logMessage, string type)
        {
            try
            {
                var logFileName = "Log" + (IsDailyLogginOn ? DateTime.Now.ToString("yyyy-MM-dd") + ".txt" : ".txt");
                var todaysLogFilePath = Path.Combine(LogPath, logFileName);
                LogToFile(type, logMessage, todaysLogFilePath);
            }
            catch (Exception ex)
            {
                //log log-exception somewhere else if required!
            }
        }

        static void LogToFile(string logType, string logMessage, string logFilePath)
        {
            lock (_syncObject)
            {
                File.AppendAllText(logFilePath,
                        string.Format("Logged on: {1} at: {2}{0}{3}: {4}{0}--------------------{0}",
                        Environment.NewLine, DateTime.Now.ToLongDateString(),
                        DateTime.Now.ToLongTimeString(), logType, logMessage));
            }
        }
    }
}
