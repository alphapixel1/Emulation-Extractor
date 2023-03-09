using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulation_Extractor.Classes
{
    internal class GameFilesDestination
    {
        public GameFilesDestination(string destination)
        {
            GameFiles = new();
            ErrorFiles = new();
            Destination = destination;
        }

        public List<GameFile> GameFiles { get; }
        public List<GameFile> ErrorFiles { get; }
        public string Destination { get; }
    }
}
