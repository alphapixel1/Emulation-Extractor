using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulation_Extractor
{
    public class GameFile
    {
        public bool isZip=false;
        public EmulatorClass? Emulator { get; set; }
        public string EmulatorName
        {
            get
            {
                if(Emulator == null)
                    return isZip? "Zip":"None";
                return Emulator!.Name;
            }
        }
        public string NameNoExtension { get; set; }
        public string FilePath { get; set; }

        public GameFile(string path,bool isZip)
        {
            this.isZip = isZip;
            Emulator = EmulatorClass.FindEmulator(path);
            FilePath = path;
            NameNoExtension = System.IO.Path.GetFileNameWithoutExtension(path);
        }
        public GameFile(string path)
        {
            this.isZip = DirectoryScanner.isFileZip(path);
            Emulator = EmulatorClass.FindEmulator(path);
            FilePath = path;
            NameNoExtension = System.IO.Path.GetFileNameWithoutExtension(path);
        }
    }
    
}
