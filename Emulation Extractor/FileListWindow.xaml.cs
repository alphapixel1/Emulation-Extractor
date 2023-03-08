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
        public List<GameFile> GameFiles;
        public FileListWindow(List<string> files)
        {
            GameFiles=files.Select(e => new GameFile(e)).Where(e=>e.Emulator!=null || e.isZip).ToList();
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileListView.ItemsSource = GameFiles;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FileListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("EmulatorName");
            //groupDescription.CustomSort =  new MyComparer();
            view.GroupDescriptions.Add(groupDescription);
            
        }
      /*  public class MyComparer : IComparer<GameFile>,IComparer
        {
            public int Compare(GameFile x, GameFile y)
            {
                if (x.isZip)
                    return -1;
                else if (y.isZip)
                    return 1;
                else if (x.EmulatorName == "None")
                    return 1;
                else if (y.EmulatorName == "None")
                    return -1;
                return 0;
            }

            public int Compare(object? x, object? y)
            {
                //CollectionViewGroupInternal
                return -1;
            }
        }*/
    }
}
