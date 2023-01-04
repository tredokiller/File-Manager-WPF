using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Task10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var drive in Directory.GetLogicalDrives())
            {
                DiskSelector.Items.Add(drive);
            }
        }

        private async void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => {
            Dispatcher.Invoke(() =>
                { 
            var item = (DiskTreeViewItem)sender;
                    
            Trace.WriteLine(item.DataPath);

            if (item.Items.Count != 1 || item.Items[0] != null)
            {
                return;
            }
            
            item.Items.Clear();

            var fullPath = item.DataPath;


            var directories = new List<string>();

            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                {
                    directories.AddRange(dirs);
                }
            }
            catch
            {
                // ignored
            }
            
            directories.ForEach(directoryPath =>
            {
                var subItem = new DiskTreeViewItem(directoryPath, GetFileFolderName(directoryPath) , item);

                subItem.Items.Add(null);
               item.Items.Add(subItem);

               Folder_Expanded(subItem, new RoutedEventArgs());

            });

            var files = new List<string>();

            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                {
                    files.AddRange(fs);
                }
            }
            catch
            {
                // ignored
            }

            files.ForEach(filePath =>
            {
                var subItem = new DiskTreeViewItem(filePath, GetFileFolderName(filePath) , item);
                item.Items.Add(subItem);
            });
            });
            });
        }



        public static string GetFileFolderName(string path)
        {
            //Get last part of the backslash
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var normalizedPath = path.Replace('/', '\\');

            var lastIndex = normalizedPath.LastIndexOf('\\');

            if (lastIndex <= 0)
            {
                return path;
            }


            return path.Substring(lastIndex + 1);

        }

        private void Scanner_OnClick(object sender, RoutedEventArgs e)
        {
            var scannerButton = (Button)sender;
            
            if (DiskSelector.Text == String.Empty)
            {
                string warningMessageText = "No drive selected for scanning";
                string caption = "Drive not found";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(warningMessageText, caption, button, icon);

            }

            else
            {
                var item = new DiskTreeViewItem(DiskSelector.Text, DiskSelector.Text);

                item.Items.Add(null);
                
                FolderView.Items.Add(item);

                Folder_Expanded(item, new RoutedEventArgs());
//asd
            }
        }
    }
}