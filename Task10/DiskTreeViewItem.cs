using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Controls;

namespace Task10;

public class DiskTreeViewItem : TreeViewItem , INotifyPropertyChanged
{
    
    private readonly bool _isDrive;
    
    private string _sizeOfFolder;

    private static string[] _sizeSyms = {"B" , "KB" , "MB" , "GB" , "TB"};




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
        
        AddSizeToDir(dirSize);
    }



    private long CalculateDirectorySize(DirectoryInfo d)
    {
        long size = 0;

        try
        {
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }

            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += CalculateDirectorySize(di);
            }
        }
        catch

        {
            // ignored
        }

        AddSizeToDir(size);
        return size;   
    }

    private void CalculateFileSize()
    {
        {
            FileInfo file = new FileInfo(DataPath);
            AddSizeToDir(file.Length);
        }
       
    }
    
    private void AddSizeToDir(long size)
    {
        double convertedSize = size;
        int sizeIndex = 0;
        for (int i = 0; i < _sizeSyms.Length; i++)
        {
            if (convertedSize > 1024)
            {
                sizeIndex += 1;
                convertedSize /= 1024;
            }
        }

        SizeOfFolder = Math.Round(convertedSize, 2) + _sizeSyms[sizeIndex];
    }
    
    private void RaisePropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
    

    public event PropertyChangedEventHandler? PropertyChanged;

    
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
    
}