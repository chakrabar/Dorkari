using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dorkari.Helpers.Files
{
    public class FileHelper
    {
        static object locker = new object();

        public static string WriteToFile(string data, string directory, string fileName = null, string fileExtension = null)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Directory not found : " + directory);
            var filePath = directory + GetFileNameWithTimeStamp(fileName, fileExtension);
            File.AppendAllText(filePath, data);
            return filePath;
        }

        public static string GetUserDirectory()
        {
            string userProfPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (string.IsNullOrEmpty(userProfPath))
                userProfPath = Path.GetTempPath(); //use system, in case user path not set
            return userProfPath;
        }

        public static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public static string GetFullPath(string directory, string filename)
        {
            return Path.Combine(directory, filename.TrimStart('/', '\\'));
        }

        public static string GetTempFileNameWithTimeStamp(string fileName, string fileExtension, out string createdTempDirectory, bool isDirectory = false)
        {
            createdTempDirectory = GetTemporaryDirectory();
            var newFileName = GetFileNameWithTimeStamp(fileName, fileExtension, isDirectory);
            return GetFullPath(createdTempDirectory, newFileName);
        }

        public static void DeleteAllDirectories(params string[] directories)
        {
            foreach (var drt in directories)
            {
                if (Directory.Exists(drt))
                    Directory.Delete(drt, true);
            }
        }

        public static void DeleteFilesAndFoldersRecursively(string directoryPath)
        {
            foreach (string fil in Directory.GetFiles(directoryPath))
            {
                System.Threading.Thread.Sleep(1);
                File.Delete(fil);
            }
            foreach (string subDrt in Directory.GetDirectories(directoryPath))
            {
                DeleteFilesAndFoldersRecursively(subDrt);
            }
            System.Threading.Thread.Sleep(2); //Sleep(0) is not enough. This increases chance of successful delete
            Directory.Delete(directoryPath);
        }

        public static string AddTextFilesToDirectory(string directoryName, string directoryPath, string directoryPrefix, List<FileInfo> files)
        {
            return AddFilesToDirectory(directoryName, directoryPath, directoryPrefix, files, WriteTextFileToDirectory);
        }

        public static string AddBinaryFilesToDirectory(string directoryName, string directoryPath, string directoryPrefix, List<FileInfo> files)
        {
            return AddFilesToDirectory(directoryName, directoryPath, directoryPrefix, files, WriteBinaryFileToDirectory);
        }

        public static void AppendToFile(string filePath, string content)
        {
            lock (locker)
            {
                File.AppendAllText(filePath, content);
            }
        }

        public static string GetFileSearchPattern(string fileNamePart, string extension = "xml")
        {
            if (!string.IsNullOrEmpty(fileNamePart))
                return "*" + fileNamePart.Trim() + "*." + extension;
            return string.Empty;
        }

        public static string GetFileNameWithTimeStamp(string fileName, string extension, bool isDirectory = false) //TODO: improve
        {
            var file = string.IsNullOrEmpty(fileName) ? @"\DorkariFile_" : @"\" + fileName + "_";
            file = file + DateTime.Now.ToString("yyyyMMMMdd") + "_" + DateTime.Now.ToString("HH.mm.ss.ffffff");
            return isDirectory ? file : file + "." + (string.IsNullOrEmpty(extension) ? "txt" : extension);
        }

        public static string GetSingleDirectoryWithFiles(string rootDirectory)
        {
            string directoryName = string.Empty;
            string[] subDirectories = Directory.GetDirectories(rootDirectory);

            if (subDirectories != null && subDirectories.Count() == 1) //Files are placed under directory and then zipped into a directory.
                directoryName = subDirectories[0];
            else
            {
                var files = Directory.GetFiles(rootDirectory); //Xmls are directly zipped into a directory.
                if (files != null && files.Count() > 0)
                    directoryName = rootDirectory;
                else
                    throw new Exception("Extracted directory was empty!"); //no files found after extrcting a given zip file.
            }
            return directoryName;
        }

        #region PrivateMethods

        private static string AddFilesToDirectory(string directoryName, string directoryPath, string directoryPrefix,
                                                        List<FileInfo> files, Action<List<FileInfo>, string> writer)
        {
            string directoryFullPath = string.Empty;
            if (!string.IsNullOrEmpty(directoryName) && !string.IsNullOrEmpty(directoryPath) && files != null && files.Count > 0)
            {
                directoryFullPath = Path.Combine(directoryPath, directoryPrefix + directoryName);
                string tmpDirectory = directoryFullPath;

                if (!Directory.Exists(directoryFullPath))
                    tmpDirectory = Directory.CreateDirectory(directoryFullPath).FullName;

                writer(files, tmpDirectory);
            }
            return directoryFullPath;
        }

        private static void WriteTextFileToDirectory(List<FileInfo> files, string directory)
        {
            foreach (var file in files)
                File.WriteAllText(Path.Combine(directory, file.Name), File.ReadAllText(file.FullName));
        }

        private static void WriteBinaryFileToDirectory(List<FileInfo> files, string directory)
        {
            foreach (var file in files)
                File.WriteAllBytes(Path.Combine(directory, file.Name), File.ReadAllBytes(file.FullName));
        }

        #endregion
    }
}
