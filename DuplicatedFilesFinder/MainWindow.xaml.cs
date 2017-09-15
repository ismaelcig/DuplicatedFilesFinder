using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.IO;

namespace DuplicatedFilesFinder
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Model.FileItem> Files = new List<Model.FileItem>();
        List<AuxObj> aos = new List<AuxObj>();
        List<Model.FileItem> DuplicatedFiles = new List<Model.FileItem>();

        public MainWindow()
        {
            InitializeComponent();
        }//TODO: Add ProgressBar

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.EnsurePathExists = true;
            cofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            cofd.IsFolderPicker = true;
            cofd.Title = "Select Folder";
            cofd.ShowHiddenItems = true;
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Load Files and Directories in TreeView
                var itemProvider = new ItemProvider();
                var items = itemProvider.GetItems(cofd.FileName);
                DataContext = items;
                //Search for any repeated files and show them in dataGrid
                var dirInfo = new DirectoryInfo(cofd.FileName);
                foreach (var item in items)
                {
                    if (item.GetType().Equals(typeof(Model.DirectoryItem)))
                    {//If it is a folder
                        Model.DirectoryItem folder = (Model.DirectoryItem)item;
                        //Console.WriteLine(item.Name + ": ");
                        //foreach (var folderItem in folder.Items)
                        //{
                        //    if (folderItem.GetType().Equals(typeof(Model.FileItem)))
                        //    {
                        //        Console.WriteLine("\t" + folderItem.Name);
                        //    }
                        //}
                    }
                    else
                    {//If it's a file
                        Model.FileItem f = (Model.FileItem)item;
                        Files.Add(f);
                        //Console.WriteLine("File: " + item.Name);
                    }
                }
                foreach (Model.FileItem file in Files)
                {
                    MD5 md5 = MD5.Create();
                    var stream = File.OpenRead(file.Path);
                    AuxObj ao = new AuxObj();
                    ao.checksum = md5.ComputeHash(stream);
                    ao.files.Add(file);//Add that file to the auxobj.files list
                    //TODO: Compare files byte-by-byte


                    /*
                    bool equal = true;
                    int cont = 0;
                    foreach (AuxObj aobj in aos)
                    {
                        foreach (byte b in aobj.checksum)
                        {
                            if (aobj.checksum[cont] == ao.checksum[cont])
                            {
                                cont++;
                            }
                            else
                            {
                                equal = false;
                                //break;
                            }
                        }
                        
                    }

                    if (!equal)
                    //if (aos.Where(c=>c.checksum == ao.checksum).Count() == 0)
                    {//If that checksum is not on the list
                        aos.Add(ao);//Add auxobj to the aos list
                    }
                    else
                    {//Add file to the auxobj.files list
                        aos.Single(c => c.checksum == ao.checksum).files.Add(file);
                    }
                    Console.Write(file.Name + ":\t");
                    foreach (byte b in ao.checksum)
                    {
                        Console.Write(b);
                    }
                    Console.WriteLine(";");*/
                }
                //If there's a checksum associated to more than 1 file, show all the files
                foreach (AuxObj aobj in aos)
                {
                    if (aobj.files.Count > 1)
                    {
                        DuplicatedFiles.AddRange(aobj.files);
                    }
                }
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = DuplicatedFiles;

                //dataGrid.ItemsSource = new DirectoryInfo(cofd.FileName).GetFiles();

            }
        }

        
    }
}
