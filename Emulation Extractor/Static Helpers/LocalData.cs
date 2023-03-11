using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emulation_Extractor
{
    internal class LocalData
    {
        public static string ProgramDataPath => Application.LocalUserAppDataPath;
        public static bool DoesLocalFileExist(string path)
        {
            try
            {
                return File.Exists(ProgramDataPath+"\\" + path);
            }
            catch
            {
                return false;
            }
        }

        internal static bool SaveFile(string fileName, string content)
        {
            try
            {
                StreamWriter streamWriter = File.CreateText(ProgramDataPath+"\\"+fileName);
                streamWriter.WriteLine(content);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal static string getFileText(string path) => File.ReadAllText(ProgramDataPath+"\\"+path, Encoding.UTF8);
    }
}
