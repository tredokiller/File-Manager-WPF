using System.IO;
using System.Windows;
using Task10.ViewModels;

namespace Task10.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(DiskSelector , DataView);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                DiskSelector.Items.Add(drive);
            }
        }
        
    }
}