using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading;

namespace Emulation_Extractor
{
    internal class DirectoryTools
    {
        public static List<string> getFiles(string startingDirectory)
        {
            var f=Directory.GetFiles(startingDirectory);
            var fileList=f.ToList();
            
            foreach (var dir in Directory.GetDirectories(startingDirectory))
            {
                fileList.AddRange(getFiles(dir));
            }
            return fileList;
        }
        public static bool isFileZip(string path) => Path.GetExtension(path).EndsWith(".zip");

        public struct UnzipUpdate
        {
            public string fileName;
            public int remainingFiles;
            public string? errorMessage;
        }
        public static async void UnzipFiles(List<GameFile> zipFiles, EventHandler<UnzipUpdate> updateHandler,EventHandler<int> completionHandler,bool deleteZipAfter)
        {
            await Task.Run(() =>
            {
                var failedExtracts = 0;
                for (int i = 0; i < zipFiles.Count; i++)
                {
                    GameFile? zipFile = zipFiles[i];
                    try
                    {
                        ZipFile.ExtractToDirectory(zipFile.FilePath, Path.GetDirectoryName(zipFile.FilePath));
                        Thread.Sleep(20);
                        File.Delete(zipFile.FilePath);
                        updateHandler.BeginInvoke(zipFile, new UnzipUpdate()
                        {
                            fileName = zipFile.NameNoExtension,
                            remainingFiles = zipFiles.Count - i,
                        }, null, null);
                    }
                    catch
                    {
                        failedExtracts++;
                        updateHandler.BeginInvoke(zipFile, new UnzipUpdate()
                        {
                            fileName = zipFile.NameNoExtension,
                            remainingFiles = zipFiles.Count - i,
                            errorMessage = "An Error Occured During Unzipping"
                        }, null, null);
                    }
                }
                completionHandler.BeginInvoke(null, failedExtracts, null, null);
            });
        }
    }
}
