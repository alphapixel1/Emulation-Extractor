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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmulatorEditor.parent = this;
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
            
            if(selectedEmulator!=null)
                EmulatorEditor.SetEmulator(selectedEmulator);
        }

    /*    private void deleteEmulator_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmulator != null &&
                MessageBox.Show("Are you sure you want to delete \""+selectedEmulator.Name+"\"?", "Delete Emulator", MessageBoxButton.YesNo,MessageBoxImage.Warning)==MessageBoxResult.Yes)
            {
                LocalEmulators.Remove(selectedEmulator);
                if (LocalEmulators.Count > 0)
                    EmulatorListView.SelectedIndex = 0;
                //reloadListView();
            }
        }*/

        /*private void modifyEmulator_Click(object sender, RoutedEventArgs e)
        {
            var se = selectedEmulator;
            if (se != null)
            {
                var maew = new ModifyAddEmulatorWindow(se);
                maew.ShowDialog();
                if (maew.ChangesSaved)
                {
                    LocalEmulators.Remove(se);
                    LocalEmulators.Insert(0, maew.emulatorClass!);
                    EmulatorListView.SelectedItem = maew.emulatorClass;
                }

            }
        }*/


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EmulatorClass.Emulators.All(e => LocalEmulators!.FirstOrDefault(z => z.IsEqualValue(e)) != null)) {
                var r = MessageBox.Show("Exiting without saving will discard all changes.\n Are you sure?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (r == MessageBoxResult.Yes)
                    Close();
            } else
                Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            EmulatorClass.SaveNewEmulators(LocalEmulators.ToList());
            ChangesSaved = true;
            Close();
        }

        public void NewEmulator()
        {
            var maew = new ModifyAddEmulatorWindow();
            maew.ShowDialog();
            if (maew.ChangesSaved)
            {
                LocalEmulators.Insert(0, maew.emulatorClass!);
                EmulatorListView.SelectedItem = maew.emulatorClass;
            }
        }
        public void UpdateEmulator(EmulatorClass oldEmulator, EmulatorClass newEmulator)
        {
            LocalEmulators.Remove(oldEmulator);
            LocalEmulators.Insert(0, newEmulator);
            EmulatorListView.SelectedItem = newEmulator;
        }
        public void DeleteEmulator(EmulatorClass e)
        {
            if(MessageBox.Show("Are you sure you want to delete \"" + selectedEmulator!.Name + "\"?", "Delete Emulator", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                LocalEmulators.Remove(e);
                if (LocalEmulators.Count > 0)
                    EmulatorListView.SelectedIndex = 0;
                else
                    EmulatorEditor.Blur(true);
            }
            
            
        }

        private void ResetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Reseting reverts all emulators to the preset.\n Do you want to continue?", "Reset Warning",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                LocalEmulators = new ObservableCollection<EmulatorClass>(EmulatorClass.GetDefaultEmulators());
                reloadListView();
            }
        }

        private void EmulatorListView_Selected(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(sender.GetType().Name+",,,,,,,,,,,,,");
        }
    }
}
