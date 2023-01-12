using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Controls;

namespace Task10.Models;

public class DiskTreeViewItem : TreeViewItem , INotifyPropertyChanged
{
    
    public string DataPath { get; }
    
    public string SizeOfFolder
    {
        set
        {
            _sizeOfFolder = value;
            RaisePropertyChanged("SizeOfFolder");
        }
        get
        {
            return _sizeOfFolder;
        }
    }
    
    public string DataName { get; }
    
    
    private readonly bool _isDrive;
    
    private static string[] _sizeSyms = {"B" , "KB" , "MB" , "GB" , "TB"};
    
    
    
    private string _sizeOfFolder;

    private long _sizeOfFolderLong;
    



    public DiskTreeViewItem(string path)
    {
        DataPath = path ?? throw new ArgumentNullException(nameof(path));
        DataName = GetDataName(DataPath);
        
        if (DataName == String.Empty)
        {
            DataName = DataPath;
            _isDrive = true;

        }
        
        Thread calculateSizeThread = new Thread(CalculatePathSize);
        calculateSizeThread.Start();

    }


    public static string GetDataName(string path)
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


    private void CalculatePathSize()
    {
        if (_isDrive)
        {
            CalculateDriveSize();
        }

        else
        {
            FileAttributes attribute = File.GetAttributes(DataPath);
            
            if (attribute.HasFlag(FileAttributes.Directory))
            {
                CalculateDirectorySize(new DirectoryInfo(DataPath));
            }
            else
            {
                CalculateFileSize();
            }
        }
        
    }

    
    private void CalculateDriveSize()
    {
        long dirSize = 0;
        DriveInfo drive = new DriveInfo(DataPath);

        if (drive.IsReady)
        {
            dirSize = drive.TotalSize - drive.AvailableFreeSpace;
        }

        _sizeOfFolderLong = dirSize;
        
        AddSizeToItem();
    }

    
    private void CalculateDirectorySize(DirectoryInfo d)
    {
        try
        {
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                _sizeOfFolderLong += fi.Length;
            }

            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                CalculateDirectorySize(di);
            }
        }
        catch

        {
            // ignored
        }

        AddSizeToItem();
    }


    private void CalculateFileSize()
    {
        {
            FileInfo file = new FileInfo(DataPath);
            _sizeOfFolderLong = file.Length;
            
            AddSizeToItem();
        }
       
    }
    
    private void AddSizeToItem()
    {
        int sizeIndex = 0;
        double size = _sizeOfFolderLong;
        for (int i = 0; i < _sizeSyms.Length; i++)
        {
            if (size > 1024)
            {
                sizeIndex += 1;
                size /= 1024;
            }
            else
            {
                break;
            }
            
        }
        SizeOfFolder = Math.Round(size, 2) + _sizeSyms[sizeIndex];
    }
    
    
    
    
    
    
    private void RaisePropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}