using Dorkari.Helpers.Files;
using Dorkari.Helpers.Serialization;
using System;
using System.IO;
using System.Reflection;

namespace Dorkari.Framework.Logging
{
    class FileLogWriter
    {
        public static void Add(LogEntry log)
        {
            var binPath = Assembly.GetExecutingAssembly().CodeBase;
            string localBinPath = new Uri(binPath).LocalPath;
            var pathToBin = Path.GetDirectoryName(localBinPath);

            string fileName = FileHelper.GetFileNameWithDate("LogFile_Unity", "log");
            string fullLogFilePath = FileHelper.GetFullPath(pathToBin, fileName);

            FileHelper.AppendToFile(fullLogFilePath, new JsonHelper().SerializeData(log, false));
        }
    }
}
