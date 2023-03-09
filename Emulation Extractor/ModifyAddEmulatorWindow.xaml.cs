using System;
using System.Collections.Generic;
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
    /// Interaction logic for ModifyAddEmulatorWindow.xaml
    /// </summary>
    public partial class ModifyAddEmulatorWindow : Window
    {
        public EmulatorClass? emulatorClass;
        public bool ChangesSaved = false;
        public ModifyAddEmulatorWindow()
        {
            InitializeComponent();
        }
        public ModifyAddEmulatorWindow(EmulatorClass em)
        {
            emulatorClass = em.Clone();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (emulatorClass != null) {
                Title = "Modify Emulator";
                EmulatorNameTextBox.Text= emulatorClass.Name;
                FileTypesListBox.ItemsSource = emulatorClass.FileTypes;
                OutputDirectoryTextBlock.Text = emulatorClass.OutputDirectoryBindingStr;
            }
            else
            {
                Title = "New Emulator";
                emulatorClass = new EmulatorClass("",new());
            }
        }

        private void Clear_FileTypes(object sender, RoutedEventArgs e)
        {
            emulatorClass!.FileTypes.Clear();
            refreshFileTypes();
        }

        private void RemoveFiletype_Click(object sender, RoutedEventArgs e)
        {
            if (FileTypesListBox.SelectedItem != null)
            {
                emulatorClass!.FileTypes.Remove((string)FileTypesListBox.SelectedItem);
                refreshFileTypes();
            }
        }

        private void AddFileType_Click(object sender, RoutedEventArgs e)
        {
            var t = FileTypeTextBox.Text.Replace(".","").Replace(" ","");
            if(t!=string.Empty && !emulatorClass!.FileTypes.Contains(t))
            {
                emulatorClass.FileTypes.Add(t);
            }
            refreshFileTypes();
        }
        private void refreshFileTypes()
        {
            FileTypesListBox.ItemsSource = new List<object>();
            FileTypesListBox.ItemsSource = emulatorClass!.FileTypes;
        }

        private void Clear_OutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            emulatorClass!.OutputDirectory = String.Empty;
            OutputDirectoryTextBlock.Text = emulatorClass!.OutputDirectoryBindingStr;

        }

        private void ChangeOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result.ToString() != String.Empty && fbd.SelectedPath != String.Empty)
                {
                    emulatorClass!.OutputDirectory = fbd.SelectedPath;
                    OutputDirectoryTextBlock.Text = emulatorClass!.OutputDirectoryBindingStr;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (EmulatorNameTextBox.Text != String.Empty)
            {
                this.Close();
                emulatorClass!.Name = EmulatorNameTextBox.Text;
                ChangesSaved = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Name cannot be empty.","Name Empty",MessageBoxButton.OK,MessageBoxImage.Error);
            }            
        }
    }
}
