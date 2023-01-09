using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                DiskSelector.Items.Add(drive);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {

            if (sender == null)
            {
                throw new ArgumentNullException();
            }


            if (sender.GetType() != typeof(DiskTreeViewItem))
            {
                throw new ArgumentException();
            }
            
            
            var item = (DiskTreeViewItem)sender;
            
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
                var subItem = new DiskTreeViewItem(directoryPath);

                subItem.Expanded += Folder_Expanded;

                subItem.Items.Add(null);
                item.Items.Add(subItem);
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
                var subItem = new DiskTreeViewItem(filePath);
                item.Items.Add(subItem);
            });
        }

        

        private void Scanner_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException();
            }

            if (sender.GetType() != typeof(Button))
            {
                throw new ArgumentException();
            }
            
            if (DiskSelector.Text == String.Empty)
            {
                CreateWarningButton();
            }

            else
            {
                foreach (var drive in DataView.Items.OfType<DiskTreeViewItem>())
                {
                    if (drive.DataName == DiskSelector.Text)
                    {
                        DataView.Items.RemoveAt(DataView.Items.IndexOf(drive));
                        break;
                    }
                }

                var item = new DiskTreeViewItem(DiskSelector.Text);

                item.Expanded += Folder_Expanded;


                item.Items.Add(null);
                
                DataView.Items.Add(item);

                item.IsExpanded = true;

            }
        }



        private void CreateWarningButton()
        {
            string warningMessageText = "No drive selected for scanning";
            string caption = "Drive not found";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBox.Show(warningMessageText, caption, button, icon);
        }
    }
}