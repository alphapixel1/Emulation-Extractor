using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Emulation_Extractor.DirectoryTools;

namespace Emulation_Extractor
{
    /// <summary>
    /// Interaction logic for ExtractionProgress.xaml
    /// </summary>
    public partial class ExtractionProgress : Window
    {
        private readonly string scanDirectoy;
        private readonly bool? deleteZips;
        private List<GameFile> GameFiles { get; }

        public ExtractionProgress(List<GameFile> gameFiles,string ScanDirectoy, bool? deleteZips)
        {
            GameFiles = gameFiles;
            scanDirectoy = ScanDirectoy;
            this.deleteZips = deleteZips;
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            DirectoryTools.UnzipFiles(GameFiles, deleteZips.HasValue? deleteZips.Value:false,this);
        }
        public delegate void updateProgress(UnzipUpdate update);
        public event updateProgress ProgressUpdate;

        public void changeProgress(UnzipUpdate update)
        {
            ProgressBarCompletion.Value = Math.Floor((update.remainingFiles / GameFiles.Count) * 100.0);
            UpdateTextBlock.Text = "("+(GameFiles.Count - update.remainingFiles) +"/"+ GameFiles.Count +") " + update.fileName;
            Debug.WriteLine("\t"+update.fileName);
        }
        public void extractionComplete(UnzipUpdate update)
        {
            if (update.remainingFiles > 0)
            {
                this.Close();
                MessageBox.Show("Extraction Complete", "(" + update.remainingFiles + ") Failed to be extracted");
            }
            else
            {
                this.Close();
                var gFiles= DirectoryTools.getGameFiles(scanDirectoy);
                new FileListWindow(gFiles, scanDirectoy).ShowDialog();
            }
        }
    }
}
