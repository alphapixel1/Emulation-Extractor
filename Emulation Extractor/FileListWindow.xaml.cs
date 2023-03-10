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
using System.IO;
using System.Diagnostics;

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
            GameFiles=files.Where(e=>e.Emulator!=null).ToList();
            InitializeComponent();
            this.scanDirectory = scanDirectory;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileListView.ItemsSource = GameFiles;
            HashSet<EmulatorClass> ems = new(GameFiles.Select(e => e.Emulator!));
            //EmulatorListBox.ItemsSource= ems;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FileListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("EmulatorName");
            //groupDescription.CustomSort =  new MyComparer();
            view.GroupDescriptions.Add(groupDescription);
            
        }

        private void ConfigureEmulatorOutputBtn_Click(object sender, RoutedEventArgs e)
        {
            var esw=new EmulatorSetupWindow();
            esw.ShowDialog();
            if (esw.ChangesSaved)
            {
                Close();
                var gFiles = DirectoryTools.getGameFiles(scanDirectory);
                new ZipHelperWindow(gFiles, scanDirectory).ShowDialog();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Maybe add an option to auto sort into prexisting folders and make assumptions idk");
            MessageBox.Show("use Expanding thing to show what files went where with a link to open it in explorer");
            var moveRoms = MoveRomRadio.IsChecked==true;
            List<GameFilesDestination> destinations = new();

            if (SetDirectoriesRadio.IsChecked == true)
            {
                throw new NotImplementedException();
            }
            else if (AutoRadio.IsChecked==true)
            {
                var dName=DirectoryTools.GetAvailableFolderName("Roms", scanDirectory);
                
                try
                {
                    var path = scanDirectory + "\\" + dName;
                    try
                    {
                        var directory = Directory.CreateDirectory(scanDirectory + "\\" + dName).FullName;
                        foreach (var g in GameFiles)
                        {
                            var targetFolder = directory + "\\" + g.Emulator.Name;
                            var dest = destinations.FirstOrDefault(e => e.Destination.Equals(targetFolder));
                            if (dest == null)
                            {
                                dest = new(targetFolder);
                                destinations.Add(dest);
                            }
                            try
                            {
                                

                                if (!Directory.Exists(targetFolder))
                                    Directory.CreateDirectory(targetFolder);
                                if (moveRoms)
                                    File.Move(g.FilePath, targetFolder + "\\" + g.NameWithExtension);
                                else
                                    File.Copy(g.FilePath, targetFolder + "\\" + g.NameWithExtension);
                                
                                dest.GameFiles.Add(g);
                            }
                            catch (Exception ex)
                            {
                                
                                dest.ErrorFiles.Add(new GameFilesDestination.ErrorFileReason() {GameFile=g,Reason=ex.Message });
                            }
                        }
                        MessageBox.Show("Done");
                    }
                    catch
                    {
                        MessageBox.Show("Output Directory could not be found\n Maybe scan folder has been deleted", "IO Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch
                {

                }
            }
            else //all in 1 folder
            {
                var dName=DirectoryTools.GetAvailableFolderName("Roms", scanDirectory);
                try
                {
                    var path = scanDirectory + "\\" + dName;
                    var directory=Directory.CreateDirectory(scanDirectory + "\\" + dName).FullName;
                    var destinationClass = new GameFilesDestination(directory);
                    foreach (var g in GameFiles)
                    {
                        try
                        {
                            if(moveRoms)
                                File.Move(g.FilePath, directory + "\\" + g.NameWithExtension);
                            else
                                File.Copy(g.FilePath, directory+"\\"+g.NameWithExtension);
                            destinationClass.GameFiles.Add(g);
                        }
                        catch (Exception ex)
                        {
                            destinationClass.ErrorFiles.Add(new GameFilesDestination.ErrorFileReason() { Reason=ex.Message,GameFile=g});
                            Debug.WriteLine("FileListWindow.xaml.cs:StartClick: Move All Files to one folder Error: " + ex.Message);
                        }
                    }
                    destinations.Add(destinationClass);
                    Debug.Write("Done Moving All Files");
                }
                catch
                {
                    MessageBox.Show("Output Directory could not be found\n Maybe scan folder has been deleted","IO Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            new FileDestinationWindow(destinations).ShowDialog();

            
        }
    }
}
