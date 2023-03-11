using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        internal static void LoadEmulators()
        {
            if (LocalData.DoesLocalFileExist("Emulators.json"))
                try
                {
                    var txt=LocalData.getFileText("Emulators.json");
                    Emulators=DesearializeEmulators(txt);
                    return;
                }
                catch
                {
                    MessageBox.Show("Unable to load roms from storage","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            Emulators = GetDefaultEmulators();
        }
        public static List<EmulatorClass> GetDefaultEmulators()
        {
            var emulatorsStr = Encoding.Default.GetString(Properties.Resources.Emulators);
            return DesearializeEmulators(emulatorsStr);
        }
        public static List<EmulatorClass> DesearializeEmulators(string text) => JsonConvert.DeserializeObject<List<EmulatorClass>>(text);
        private static String SerializeEmulators()
        {
            return JsonConvert.SerializeObject(Emulators, Formatting.Indented);
        }
        internal static void SaveNewEmulators(List<EmulatorClass> emulatorClasses)
        {
            Emulators = emulatorClasses;
            try
            {
                var succ = LocalData.SaveFile("Emulators.json", SerializeEmulators());
                if (!succ);
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Unable to save changes", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        [JsonProperty("fileType")]
        public List<String> FileTypes { get; set; }
        
        /*For Binding*/
        public string FileTypesStr { get
            {
                return string.Join(", ",FileTypes.Select(e=>"."+e));
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("outputDirectory")]
        public string OutputDirectory { get; set; }

        public string OutputDirectoryBindingStr { get {
                if (OutputDirectory == null || OutputDirectory==String.Empty)
                    return "Auto";
                return OutputDirectory;
            } 
        }
        public EmulatorClass(string name, List<string> fileTypes)
        {
            FileTypes = fileTypes;
            Name = name;
        }


        public bool isFileAccepted(string fileExtension) => FileTypes.Contains(fileExtension.ToLower());
        public override string ToString()
        {
            return "{\n" +
                "\"name\":\""+Name+"\",\n" +
                "\"fileTypes\":["+String.Join(",",FileTypes)+"]\n"+
                "}";
        }
        public EmulatorClass Clone() => new EmulatorClass(Name, new(FileTypes)) { OutputDirectory = this.OutputDirectory };

        public bool IsEqualValue(EmulatorClass e)
        {
            return e.Name==Name &&
                e.FileTypes.SequenceEqual(FileTypes) &&
                e.OutputDirectory == OutputDirectory;
        }

    }

}
