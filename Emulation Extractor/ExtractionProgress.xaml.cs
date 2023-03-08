using System;
using System.Collections.Generic;
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
            DirectoryTools.UnzipFiles(GameFiles, (a, b) =>
            {
                ProgressBarCompletion.Value = Math.Floor((b.remainingFiles / GameFiles.Count) * 100.0);
                UpdateTextBlock.Text = b.fileName;
            }, (a, b) =>
            {
                ProgressBarCompletion.Value = 100;
                this.Close();
            },true);
        }
    }
}
