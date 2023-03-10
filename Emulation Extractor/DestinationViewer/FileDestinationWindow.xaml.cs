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
    /// Interaction logic for FileDestinationWindow.xaml
    /// </summary>
    public partial class FileDestinationWindow : Window
    {
        public FileDestinationWindow(List<GameFilesDestination> destinations)
        {
            InitializeComponent();
            Destinations = destinations;
        }

        public List<GameFilesDestination> Destinations { get; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var dest in Destinations)
            {
                if(dest != null)
                    MainStack.Children.Add(new DestinationControl(dest));
            }
        }
    }
}
