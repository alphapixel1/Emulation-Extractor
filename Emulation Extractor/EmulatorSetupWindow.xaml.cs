using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Emulation_Extractor
{
    
    /// <summary>
    /// Interaction logic for EmulatorSetupWindow.xaml
    /// </summary>
    public partial class EmulatorSetupWindow : Window
    {
        public bool ChangesSaved { get; private set; }
        private ObservableCollection<EmulatorClass> LocalEmulators;
        private EmulatorClass? selectedEmulator => EmulatorListView.SelectedItem as EmulatorClass;

        public EmulatorSetupWindow()
        {
            LocalEmulators = new ObservableCollection<EmulatorClass>(EmulatorClass.Emulators!.Select(e => e.Clone()));
            InitializeComponent();
        }
        private void DisplayEmulatorData(EmulatorClass? e)
        {
            if (e == null)
            {
                EmulatorNameBlock.Text = "None";
                EmulatorNameBlock.ToolTip = "";

                EmulatorFileTypesBlock.Text = "None";
                EmulatorFileTypesBlock.ToolTip = "";

                OutputDirectoryBlock.Text = "None";
                OutputDirectoryBlock.ToolTip = "";
            }
            else 
            {

                EmulatorNameBlock.Text = e.Name;
                EmulatorNameBlock.ToolTip = e.Name;

                EmulatorFileTypesBlock.Text = e.FileTypesStr;
                EmulatorFileTypesBlock.ToolTip = e.FileTypesStr;

                OutputDirectoryBlock.Text = e.OutputDirectoryBindingStr;
                OutputDirectoryBlock.ToolTip = e.OutputDirectoryBindingStr;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            EmulatorListView.SelectionChanged += EmulatorListView_SelectionChanged;
            reloadListView();
        }
        private void reloadListView()
        {
            EmulatorListView.ItemsSource = new List<object>();
            EmulatorListView.ItemsSource = LocalEmulators;
            if (LocalEmulators.Count > 0)
                EmulatorListView.SelectedIndex = 0;
        }
        private void EmulatorListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayEmulatorData(selectedEmulator);
            /*Debug.WriteLine(sender.GetType().Name + "\\\\\\\\\\\\\\\\\\\\\\");
            ListView.select*/
        }

        private void deleteEmulator_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmulator != null &&
                MessageBox.Show("Are you sure you want to delete \""+selectedEmulator.Name+"\"?", "Delete Emulator", MessageBoxButton.YesNo,MessageBoxImage.Warning)==MessageBoxResult.Yes)
            {
                LocalEmulators.Remove(selectedEmulator);
                if (LocalEmulators.Count > 0)
                    EmulatorListView.SelectedIndex = 0;
                //reloadListView();
            }
        }

        private void modifyEmulator_Click(object sender, RoutedEventArgs e)
        {
            var se = selectedEmulator;
            if (se != null)
            {
                var maew = new ModifyAddEmulatorWindow(se);
                maew.ShowDialog();
                if (maew.ChangesSaved)
                {
                    LocalEmulators.Remove(se);
                    LocalEmulators.Add(maew.emulatorClass!);
                    EmulatorListView.SelectedItem = maew.emulatorClass;
                }

            }
        }
       

        private void CloseButton_Click(object sender, RoutedEventArgs e) => this.Close();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            EmulatorClass.SaveNewEmulators(LocalEmulators.ToList());
            ChangesSaved = true;
            this.Close();
        }

        private void NewEmulatorButton_Click(object sender, RoutedEventArgs e)
        {
            var maew = new ModifyAddEmulatorWindow();
            maew.ShowDialog();
            if (maew.ChangesSaved)
            {
                LocalEmulators.Add(maew.emulatorClass!);
                EmulatorListView.SelectedItem = maew.emulatorClass;
            }
        }
    }
}
