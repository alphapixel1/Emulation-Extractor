using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading;
using System.Windows.Threading;

namespace Emulation_Extractor
{
    public class DirectoryTools
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
        public static async void UnzipFiles(List<GameFile> zipFiles,bool deleteZipAfter,ExtractionProgress ui)
        {
            await Task.Run(() =>
            {
                var failedExtracts = 0;
                for (int i = 0; i < zipFiles.Count; i++)
                {
                    GameFile? zipFile = zipFiles[i];
                    try
                    {
                        //Debug.WriteLine(Path.GetDirectoryName(zipFile.FilePath));
                        var p=Path.GetDirectoryName(zipFile.FilePath) + "\\" + GetAvailableFolderName(zipFile.NameNoExtension, zipFile.FilePath);
                        ZipFile.ExtractToDirectory(zipFile.FilePath, p);
                        /*if (Directory.Exists(Path.GetDirectoryName(zipFile.FilePath) + "\\" + zipFile.NameNoExtension))
                        {
                            var z = 1;
                            while (true)
                            {
                                if (!Directory.Exists(Path.GetDirectoryName(zipFile.FilePath) + "\\" + zipFile.NameNoExtension + "(" + z + ")"))
                                    break;
                                z++;
                            }
                            ZipFile.ExtractToDirectory(zipFile.FilePath, Path.GetDirectoryName(zipFile.FilePath) + "\\" + zipFile.NameNoExtension + "(" + z + ")");
                        }
                        else
                        {
                            ZipFile.ExtractToDirectory(zipFile.FilePath, Path.GetDirectoryName(zipFile.FilePath) + "\\" + zipFile.NameNoExtension);
                        }*/
                        Thread.Sleep(25);
                        ui.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            ui.changeProgress(new UnzipUpdate()
                            {
                                fileName = zipFile.NameNoExtension,
                                remainingFiles = zipFiles.Count- i,
                            });
                        }));
                        
                      
                    }
                    catch
                    {
                        failedExtracts++;
                    }
                }
                ui.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    ui.extractionComplete(new UnzipUpdate()
                    {
                        remainingFiles = failedExtracts
                    });
                }));
                //completionHandler.BeginInvoke(null, failedExtracts, null, null);
            });
        }
        public static List<GameFile> getGameFiles(string directory) => getFiles(directory).Select(e => new GameFile(e)).ToList();


        public static string GetAvailableFolderName(string  name, string parentFolderPath)
        {
            if (!Directory.Exists(parentFolderPath + "\\" + name))
                return name;
            var i = 1;
            while (Directory.Exists(parentFolderPath + "\\" + name + " (" + i + ")"))
                i++;
            return name + " (" + i + ")";
        }

    }
}
