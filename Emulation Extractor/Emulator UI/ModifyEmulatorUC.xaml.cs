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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emulation_Extractor
{
    /// <summary>
    /// Interaction logic for ModifyEmulatorUC.xaml
    /// </summary>
    public partial class ModifyEmulatorUC : System.Windows.Controls.UserControl
    {
        private EmulatorClass? currentEmulator,emulatorCopy;
        public EmulatorSetupWindow? parent { set; private get; }
        public ModifyEmulatorUC()
        {
            InitializeComponent();
        }
        
        public void SetEmulator(EmulatorClass e)
        {
            currentEmulator=e;
            emulatorCopy = e.Clone();

            Blur(false);
            displayEmulator(e);
                
        }
        private void displayEmulator(EmulatorClass e)
        {
            EmulatorNameTextBox.Text = e.Name;
            FileTypesListBox.ItemsSource = e.FileTypes;
            OutputDirectoryTextBlock.Text = e.OutputDirectoryBindingStr;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            parent!.UpdateEmulator(currentEmulator!, emulatorCopy!);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            displayEmulator(currentEmulator!);
            emulatorCopy = currentEmulator!.Clone();
        }


   

        private void RemoveFiletype_Click(object sender, RoutedEventArgs e)
        {
            if (FileTypesListBox.SelectedItem != null)
            {
                emulatorCopy!.FileTypes.Remove((string)FileTypesListBox.SelectedItem);
                refreshFileTypes();
            }
        }

        private void Clear_FileTypes(object sender, RoutedEventArgs e)
        {
            emulatorCopy!.FileTypes.Clear();
            refreshFileTypes();
        }

        private void AddFileType_Click(object sender, RoutedEventArgs e)
        {
            var t = FileTypeTextBox.Text.Replace(".", "").Replace(" ", "");
            if (t != string.Empty && !emulatorCopy!.FileTypes.Contains(t))
            {
                emulatorCopy.FileTypes.Add(t);
                FileTypeTextBox.Text = String.Empty;
            }
            refreshFileTypes();
        }
        private void refreshFileTypes()
        {
            FileTypesListBox.ItemsSource = new List<object>();
            FileTypesListBox.ItemsSource = emulatorCopy!.FileTypes;
        }
        private void Clear_OutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            emulatorCopy!.OutputDirectory = String.Empty;
            OutputDirectoryTextBlock.Text = emulatorCopy!.OutputDirectoryBindingStr;

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            parent!.DeleteEmulator(currentEmulator!);
        }

        private void ChangeOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result.ToString() != String.Empty && fbd.SelectedPath != String.Empty)
                {
                    emulatorCopy!.OutputDirectory = fbd.SelectedPath;
                    OutputDirectoryTextBlock.Text = emulatorCopy!.OutputDirectoryBindingStr;
                }
            }
        }

        private void NewEmulatorBtn_Click(object sender, RoutedEventArgs e)
        {
            parent?.NewEmulator();
        }

        internal void Blur(bool shouldBlur)
        {
            //this.IsEnabled = !shouldBlur;
            var shouldNotBlur = !shouldBlur;
            //NewEmulatorBtn.IsEnabled = shouldNotBlur;
            foreach (var item in new System.Windows.Controls.Control[] {CancelBtn, AddFileTypeBtn, RemoveFileTypeBtn, ClearFileTypesBtn, ChangeDirBtn, ClearDirBtn, UpdateBtn, DeleteBtn, FileTypesListBox, FileTypeTextBox, EmulatorNameTextBox})
            {
                item.IsEnabled = shouldNotBlur;
            }
           /* AddFileTypeBtn.IsEnabled = shouldNotBlur;
            RemoveFileTypeBtn.IsEnabled = shouldNotBlur;
            ClearFileTypesBtn.IsEnabled = shouldNotBlur;
            ChangeDirBtn.IsEnabled = shouldNotBlur;
            ClearDirBtn.IsEnabled = shouldNotBlur;
            UpdateBtn.IsEnabled = shouldNotBlur;
            DeleteBtn.IsEnabled = shouldNotBlur;
            FileTypesListBox.IsEnabled = shouldNotBlur;
            FileTypeTextBox.IsEnabled = shouldNotBlur;
            EmulatorNameTextBox.IsEnabled = shouldNotBlur;*/
            if (shouldBlur)
            {
                displayEmulator(new EmulatorClass("",new()));
            }
        }
    }
}
