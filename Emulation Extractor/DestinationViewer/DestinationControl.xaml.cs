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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emulation_Extractor
{
    /// <summary>
    /// Interaction logic for DestinationControl.xaml
    /// </summary>
    public partial class DestinationControl : UserControl
    {
        private readonly GameFilesDestination destination;

        public DestinationControl()
        {
            InitializeComponent();
        }

        public DestinationControl(GameFilesDestination destination)
        {
            this.destination = destination;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FolderNameTextBlock.Text = System.IO.Path.GetFileName(destination.Destination);
            PathTextBlock.Text = destination.Destination;
            PathTextBlock.MouseDown += PathTextBlock_MouseDown;
            if (destination.ErrorFiles.Count == 0)
                SuccessfulTextBlock.Text = $"({destination.GameFiles.Count})";
            else
            {
                SuccessfulTextBlock.Text = $"({destination.GameFiles.Count}/{destination.GameFiles.Count + destination.ErrorFiles.Count})";
                SuccessfulTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 7, 7));
            }

            var white = this.FindResource("TextColor") as SolidColorBrush;
            //Displaying Successful Transfers
            for (int z = 0; z < destination.GameFiles.Count; z++)
            {
                GameFile? gf = destination.GameFiles[z];
                RomGrid.RowDefinitions.Add(new RowDefinition());
                var text =new string[] { gf.NameNoExtension, gf.EmulatorName, destination.Destination + "\\" + gf.NameWithExtension };
                for (int i = 0; i < 3; i++)
                {
                    
                    TextBlock block = new TextBlock() { Text = text[i] };
                    block.Foreground = white;
                    Grid.SetRow(block, z);
                    Grid.SetColumn(block, i*2);
                    RomGrid.Children.Add(block);
                }
            }
            //Displaying Error Transfers
            var startingRow = destination.GameFiles.Count;
            var errorRed= new SolidColorBrush(Color.FromRgb(255, 7, 7));
            for (int i = 0; i < destination.ErrorFiles.Count; i++)
            {
                GameFilesDestination.ErrorFileReason ef = destination.ErrorFiles[i];
                RomGrid.RowDefinitions.Add(new RowDefinition());
                var text = new string[] { ef.GameFile.NameNoExtension, ef.GameFile.EmulatorName,ef.Reason};
                for (int z = 0; z < 3; z++)
                {
                    TextBlock block = new TextBlock() { Text = text[z] };
                    block.Foreground = errorRed;
                    Grid.SetRow(block, i+startingRow);
                    Grid.SetColumn(block, z * 2);
                    RomGrid.Children.Add(block);
                }
            }
        }

        private void PathTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = destination.Destination,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        private void PathTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            PathTextBlock.Foreground = SystemColors.HighlightTextBrush;
        }

        private void PathTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            PathTextBlock.Foreground = SystemColors.ActiveCaptionBrush;
        }
    }
}
