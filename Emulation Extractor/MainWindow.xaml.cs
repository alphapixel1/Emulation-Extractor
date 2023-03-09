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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Compression;
using System.Windows.Forms;

namespace Emulation_Extractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            EmulatorClass.loadEmulators();
            InitializeComponent();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult fbdResult = fbd.ShowDialog();
                if (fbdResult.ToString() != String.Empty && fbd.SelectedPath != String.Empty)
                {
                    var gFiles = DirectoryTools.getGameFiles(fbd.SelectedPath);
                    var zips = gFiles.Count(e => e.isZip);
                    if (zips > 0)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("There are (" + zips + ") Zip files, would you like them to be unzipped before scanning?", "Zip Files", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                               // this.Close();
                                new ZipHelperWindow(gFiles, fbd.SelectedPath).ShowDialog();
                                break;
                            case MessageBoxResult.No:
                               // this.Close();
                                new FileListWindow(gFiles, fbd.SelectedPath).ShowDialog();
                                break;
                        }
                    }
                    else
                    {
                        //this.Close();
                        new FileListWindow(gFiles, fbd.SelectedPath).ShowDialog();
                    }
                }
            }

            //new ScanWindow().ShowDialog();
        }

        private void EmulatorsBtn_Click(object sender, RoutedEventArgs e)
        {
            new EmulatorSetupWindow().ShowDialog();
        }
    }
}
