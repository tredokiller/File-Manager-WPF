using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Task10.Models;

namespace Task10.ViewModels;

public class MainViewModel
{
    private readonly ComboBox _diskSelector;
    private readonly TreeView _dataView;
    
    public ICommand ScannerButtonOnClickCommand { get; }
    
    
    
    public MainViewModel(ref ComboBox diskSelector, ref TreeView dataView)
    {
        _diskSelector = diskSelector ?? throw new ArgumentNullException(nameof(diskSelector));
        _dataView = dataView ?? throw new ArgumentNullException(nameof(dataView));
        
        ScannerButtonOnClickCommand = new RelayCommand(ScannerButtonOnClick);
    }
    
    private void ScannerButtonOnClick()
    {
        if (_diskSelector.Text == String.Empty)
        {
            CreateWarningButton();
        }

        else
        {
            foreach (var drive in _dataView.Items.OfType<DiskTreeViewItem>())
            {
                if (drive.DataName == _diskSelector.Text)
                {
                    _dataView.Items.RemoveAt(_dataView.Items.IndexOf(drive));
                    break;
                }
            }

            var item = new DiskTreeViewItem(_diskSelector.Text);

            item.Expanded += DataExpanded;


            item.Items.Add(null);
                
            _dataView.Items.Add(item);

            item.IsExpanded = true;
        }
    }
    
    private void DataExpanded(object sender , RoutedEventArgs args)
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

           subItem.Expanded += DataExpanded;

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
    
    private void CreateWarningButton()
    {
        string warningMessageText = "No drive selected for scanning";
        string caption = "Drive not found";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Warning;

        MessageBox.Show(warningMessageText, caption, button, icon);
    }
}