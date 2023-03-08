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
    /// Interaction logic for ZipHelperWindow.xaml
    /// </summary>
    public partial class ZipHelperWindow : Window
    {
        private readonly string scanDirectoy;

        public ZipHelperWindow(List<GameFile> files, string ScanDirectoy)
        {
            InitializeComponent();
            GameFiles = files.Where(e=>e.isZip).ToList();
            scanDirectoy = ScanDirectoy;
        }

        public List<GameFile> GameFiles { get; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ZipListView.ItemsSource = GameFiles;
            ZipListView.SelectionMode = SelectionMode.Multiple;
            ZipListView.SelectAll();
            UpdateSelectedCount();
        }

        private void UpdateSelectedCount()
        {
            SelectedCountTextBlock.Text = "Selected Items: " + ZipListView.SelectedItems.Count;
        }
        private void SelectAllButton_Click(object sender, RoutedEventArgs e) => ZipListView.SelectAll();

        private void ZipListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateSelectedCount();

        private void DeselectAllButton_Click(object sender, RoutedEventArgs e) => ZipListView.UnselectAll();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new ScanWindow().ShowDialog();
        }

        private void ExtractAndScan_Click(object sender, RoutedEventArgs e)
        {
            var selectedGameFiles = (List<GameFile>)ZipListView.SelectedItems;
            if (selectedGameFiles.Count > 0)
            {
                this.Close();
                new ExtractionProgress(selectedGameFiles, scanDirectoy,DeleteCheckbox.IsChecked).ShowDialog();
            }
            else
            {
                
                //DirectoryTools.UnzipFiles(selectedGameFiles,)
            }
            
        }
    }
}
