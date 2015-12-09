using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace Dorkari.Helpers.Files
{
    public class Compresser //from SharpZipLib samples
    {
        public void CreateTarGZipFromDirectory(string tgzOutputFilename, string sourceDirectory)
        {
            using (Stream outStream = File.Create(tgzOutputFilename))
            using (Stream gzoStream = new GZipOutputStream(outStream))
            using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
            {
                // Note that the RootPath is currently case sensitive and must be forward slashes e.g. "c:/temp"
                // and must not end with a slash, otherwise cuts off first char of filename
                // This is scheduled for fix in next release
                tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
                if (tarArchive.RootPath.EndsWith("/"))
                    tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);

                AddDirectoryFilesToTarGZip(tarArchive, sourceDirectory);
            }
        }

        private void AddDirectoryFilesToTarGZip(TarArchive tarArchive, string sourceDirectory, bool recurse = false)
        {
            // Write each file to the tar.
            string[] filenames = Directory.GetFiles(sourceDirectory);
            foreach (string filename in filenames)
            {
                TarEntry tarEntry = TarEntry.CreateEntryFromFile(filename);
                tarArchive.WriteEntry(tarEntry, true);
            }
            if (recurse)
            {
                TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
                tarArchive.WriteEntry(tarEntry, false); //optional
                string[] directories = Directory.GetDirectories(sourceDirectory);
                foreach (string directory in directories)
                    AddDirectoryFilesToTarGZip(tarArchive, directory, recurse);
            }
        }

        public void CreateCustomTarGZipFromDirectory(string outputFileName, string sourceDirectory, bool includeSubDirectories = false, bool createEntryForSourceDirectory = false)
        {
            using (Stream outStream = File.Create(outputFileName))
            {
                using (GZipOutputStream gzoStream = new GZipOutputStream(outStream))
                {
                    gzoStream.SetLevel(3); // 1 - 9, 1 is best speed, 9 is best compression //3
                    using (TarOutputStream tarOutputStream = new TarOutputStream(gzoStream))
                        CreateTarManually(tarOutputStream, sourceDirectory, includeSubDirectories, createEntryForSourceDirectory);
                }
            }
        }

        private void CreateTarManually(TarOutputStream tarOutputStream, string sourceDirectory,
                                        bool isRecursive, bool createEntryForSourceDirectory)
        {
            using (tarOutputStream)
            {
                if (createEntryForSourceDirectory)
                {
                    TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
                    tarOutputStream.PutNextEntry(tarEntry);
                }

                string[] filenames = Directory.GetFiles(sourceDirectory);
                foreach (string filename in filenames)
                {
                    using (Stream inputStream = File.OpenRead(filename))
                    {
                        string tarName = Path.GetFileName(filename); //FROM C:\A\B\C.txt TO C.txt
                        long fileSize = inputStream.Length;

                        TarEntry entry = TarEntry.CreateTarEntry(tarName);
                        entry.Size = fileSize; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                        tarOutputStream.PutNextEntry(entry); // Add the entry to the tar stream, before writing the data.

                        byte[] localBuffer = new byte[32 * 1024]; // this is copied from TarArchive.WriteEntryCore
                        while (true)
                        {
                            int numRead = inputStream.Read(localBuffer, 0, localBuffer.Length);
                            if (numRead <= 0)
                            {
                                break;
                            }
                            tarOutputStream.Write(localBuffer, 0, numRead);
                        }
                    }
                    tarOutputStream.CloseEntry();
                }

                if (isRecursive)
                {
                    string[] directories = Directory.GetDirectories(sourceDirectory);
                    foreach (string directory in directories)
                    {
                        CreateTarManually(tarOutputStream, directory);
                    }
                }
            }
        }

        //from https://github.com/icsharpcode/SharpZipLib/wiki/GZip-and-Tar-Samples#createFull
        public void CreateTarFromDirectory(string outputFileName, string sourceDirectory)
        {
            // Create an output stream. Does not have to be disk, could be MemoryStream etc.
            Stream outStream = File.Create(outputFileName); //@"c:\temp\test.tar";

            // If you wish to create a .Tar.GZ (.tgz):
            // - set the filename above to a ".tar.gz",
            // - create a GZipOutputStream here
            // - change "new TarOutputStream(outStream)" to "new TarOutputStream(gzoStream)"
            // Stream gzoStream = new GZipOutputStream(outStream);
            // gzoStream.SetLevel(3); // 1 - 9, 1 is best speed, 9 is best compression

            TarOutputStream tarOutputStream = new TarOutputStream(outStream);

            CreateTarManually(tarOutputStream, sourceDirectory); //@"c:\temp\debug"

            // Closing the archive also closes the underlying stream.
            // If you don't want this (e.g. writing to memorystream), set tarOutputStream.IsStreamOwner = false
            tarOutputStream.Close();
        }

        private void CreateTarManually(TarOutputStream tarOutputStream, string sourceDirectory)
        {
            // Optionally, write an entry for the directory itself.
            TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
            tarOutputStream.PutNextEntry(tarEntry);

            // Write each file to the tar.
            string[] filenames = Directory.GetFiles(sourceDirectory);

            foreach (string filename in filenames)
            {
                // You might replace these 3 lines with your own stream code
                using (Stream inputStream = File.OpenRead(filename))
                {
                    string tarName = filename.Substring(3); // strip off "C:\"

                    long fileSize = inputStream.Length;
                    // Create a tar entry named as appropriate. You can set the name to anything,
                    // but avoid names starting with drive or UNC.
                    TarEntry entry = TarEntry.CreateTarEntry(tarName);

                    // Must set size, otherwise TarOutputStream will fail when output exceeds.
                    entry.Size = fileSize;

                    // Add the entry to the tar stream, before writing the data.
                    tarOutputStream.PutNextEntry(entry);

                    // this is copied from TarArchive.WriteEntryCore
                    byte[] localBuffer = new byte[32 * 1024];
                    while (true)
                    {
                        int numRead = inputStream.Read(localBuffer, 0, localBuffer.Length);
                        if (numRead <= 0)
                        {
                            break;
                        }
                        tarOutputStream.Write(localBuffer, 0, numRead);
                    }
                }
                tarOutputStream.CloseEntry();
            }

            // Recurse. Delete this if unwanted.
            string[] directories = Directory.GetDirectories(sourceDirectory);
            foreach (string directory in directories)
            {
                CreateTarManually(tarOutputStream, directory);
            }
        }

        public void ExtractZippedFiles(string zippedFilePath, string outputDirectory)
        {
            using (FileStream fs = File.OpenRead(zippedFilePath))
            using (var zf = new ZipFile(fs))
            {
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    using (Stream zipStream = zf.GetInputStream(zipEntry))
                    {
                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(outputDirectory, entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
            }
        }

        public void ExtractZipFilesCustom(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        public void CompressFilesToZip(string outZipPath, string sourceFolderPath, string password = null)
        {
            FileStream fsOut = File.Create(outZipPath);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            if (!String.IsNullOrEmpty(password))
                zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.

            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
            int folderOffset = sourceFolderPath.Length + (sourceFolderPath.EndsWith("\\") ? 0 : 1);

            CompressFolder(sourceFolderPath, zipStream, folderOffset);

            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
            zipStream.Close();
        }

        // Recurses down the folder structure
        private void CompressFolder(string sourceFolderPath, ZipOutputStream zipStream, int folderOffset)
        {
            string[] files = Directory.GetFiles(sourceFolderPath);
            foreach (string filename in files)
            {
                FileInfo fi = new FileInfo(filename);
                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(sourceFolderPath);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }

        public void ExtractTGZ(string gzArchiveFilePath, string destinationDirectory)
        {
            using (Stream inStream = File.OpenRead(gzArchiveFilePath))
            using (Stream gzipStream = new GZipInputStream(inStream))
            using (TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                tarArchive.ExtractContents(destinationDirectory);
        }
    }
}
