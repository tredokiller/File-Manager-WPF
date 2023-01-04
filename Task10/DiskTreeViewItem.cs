using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Task10;

public class DiskTreeViewItem : TreeViewItem , INotifyPropertyChanged
{
    public string DataPath { get; }
    
    
    private long _sizeOfFolder;

    private readonly DiskTreeViewItem _upperItem;
    
    public long SizeOfFolder
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
    
    public string FileFolderName { get; }



    public DiskTreeViewItem(string path , string fileName , DiskTreeViewItem upperItem = null)
    {
        DataPath = path;
        FileFolderName = fileName;
        _upperItem = upperItem;

        //Thread calculateSizeThread = new Thread(CalculatePathSize);
        //calculateSizeThread.Start();
        
        CalculatePathSize();
    }





    private async void CalculatePathSize()
    {
        await Task.Run(() =>
        {
            DirectoryInfo dirInfo = new DirectoryInfo(DataPath);
            long dirSize = 0;
            try
            {
                dirSize = dirInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).Sum(file => file.Length);
            }
            catch
            {
                // ignored
            }

            AddSizeToDir(dirSize);
        });
    }
    
    
    private void AddSizeToDir(long size)
    { 
        SizeOfFolder += size;

        if (_upperItem != null)
        {
            _upperItem.AddSizeToDir(size);
        }
    }
    
    
    
    
    
    
    
    private void RaisePropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
    

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}