using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Emulation_Extractor
{
    public class EmulatorClass
    {
        public static List<EmulatorClass>? Emulators;
        public static EmulatorClass? FindEmulator(string filePath)
        {
            var extension = Path.GetExtension(filePath).TrimStart('.');
            return Emulators?.FirstOrDefault(e => e.isFileAccepted(extension));
        }
        internal static void loadEmulators()
        {
            var emulatorsStr = Encoding.Default.GetString(Properties.Resources.Emulators);            
            Emulators=JsonConvert.DeserializeObject<List<EmulatorClass>>(emulatorsStr);
           /* foreach (var em in Emulators)
            {
                Debug.WriteLine(em.ToString());
            }*/
        }
        [JsonProperty("fileType")]
        public List<String> FileTypes { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        public EmulatorClass(string name, List<string> fileTypes)
        {
            FileTypes = fileTypes;
            Name = name;
        }


        public bool isFileAccepted(string fileExtension) => FileTypes.Contains(fileExtension);
        public override string ToString()
        {
            return "{\n" +
                "\"name\":\""+Name+"\",\n" +
                "\"fileTypes\":["+String.Join(",",FileTypes)+"]\n"+
                "}";
        }
    }

}
