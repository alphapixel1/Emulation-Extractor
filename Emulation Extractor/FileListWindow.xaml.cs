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
using System.Linq;
using System.Collections;

namespace Emulation_Extractor
{
    /// <summary>
    /// Interaction logic for FileListWindow.xaml
    /// </summary>
    public partial class FileListWindow : Window
    {
        private readonly string scanDirectory;
        public List<GameFile> GameFiles;
        public FileListWindow(List<GameFile> files,string scanDirectory)
        {
            //GameFiles= files;
            GameFiles=files.Where(e=>e.Emulator!=null || e.isZip).ToList();
            InitializeComponent();
            this.scanDirectory = scanDirectory;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileListView.ItemsSource = GameFiles;
            HashSet<EmulatorClass> ems = new(GameFiles.Select(e => e.Emulator));
            //EmulatorListBox.ItemsSource= ems;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FileListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("EmulatorName");
            //groupDescription.CustomSort =  new MyComparer();
            view.GroupDescriptions.Add(groupDescription);
            
        }
    }
}
