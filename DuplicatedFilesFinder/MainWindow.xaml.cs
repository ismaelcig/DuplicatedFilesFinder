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
                        Console.WriteLine(item.Name + ": ");
                        foreach (var folderItem in folder.Items)
                        {
                            if (folderItem.GetType().Equals(typeof(Model.FileItem)))
                            {
                                Console.WriteLine("\t" + folderItem.Name);
                            }
                        }
                    }
                    else
                    {//If it's a file
                        Model.FileItem f = (Model.FileItem)item;
                        Files.Add(f);
                        //Console.WriteLine("File: " + item.Name);
                    }
                }





                /*
                MD5 md5 = MD5.Create();
                var stream = File.OpenRead()
                
                dataGrid.ItemsSource = new DirectoryInfo(cofd.FileName).GetFiles();*/

            }
        }

        
    }
}
