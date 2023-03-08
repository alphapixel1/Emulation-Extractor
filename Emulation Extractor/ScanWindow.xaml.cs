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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emulation_Extractor
{
    /// <summary>
    /// Interaction logic for ScanWindow.xaml
    /// </summary>
    public partial class ScanWindow : Window
    {
        private string select_Dir;
        private string selectedDirectory
        {
            get
            {
                return select_Dir;
            }
            set
            {
                select_Dir = value;
                InputChangeDirLabel.Content = value;
                InputChangeDirLabel.ToolTip = value;
            }
        }
        public ScanWindow()
        {
            InitializeComponent();
        }

        private void InputChangeDirBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result.ToString() != String.Empty)
                {
                    selectedDirectory = fbd.SelectedPath;
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ScanBtn_Click(object sender, RoutedEventArgs e)
        {
            if(selectedDirectory == null)
            {
                System.Windows.MessageBox.Show("Directory must be set.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                var files=DirectoryScanner.getFiles(selectedDirectory);
                
                var gFiles= files.Select(e => new GameFile(e)).ToList();
                var zips = gFiles.Count(e=>e.isZip);
                if (zips>0)
                {
                    MessageBoxResult result= System.Windows.MessageBox.Show("There are ("+zips+") Zip files, would you like them to be unzipped before scanning?","Zip Files", MessageBoxButton.YesNoCancel,MessageBoxImage.Question);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            this.Close();
                            break;
                        case MessageBoxResult.No:
                            this.Close();
                            new FileListWindow(gFiles).ShowDialog();
                            break;
                    }
                }
                else
                {
                    this.Close();
                    new FileListWindow(gFiles).ShowDialog();
                }      
                
            }
        }
    }
}
