using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulation_Extractor
{
    public class GameFilesDestination
    {
        public struct ErrorFileReason
        {
            public GameFile GameFile;
            public String Reason;
        }
        public GameFilesDestination(string destination)
        {
            GameFiles = new();
            ErrorFiles = new();
            Destination = destination;
        }

        public List<GameFile> GameFiles { get; }
        public List<ErrorFileReason> ErrorFiles { get; }
        public string Destination { get; }
    }
}
