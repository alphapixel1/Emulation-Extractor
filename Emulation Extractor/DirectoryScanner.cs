using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Emulation_Extractor
{
    internal class DirectoryScanner
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
    }
}
