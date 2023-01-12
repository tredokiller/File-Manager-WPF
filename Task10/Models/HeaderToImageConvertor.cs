using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Task10.Models;


[ValueConversion(typeof(string), typeof(BitmapImage))]

public class HeaderToImageConvertor : IValueConverter
{

    public static HeaderToImageConvertor Instance = new HeaderToImageConvertor();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        
        var path = (string)value;

        if (path == null)
        {
            return null;
        }

        var name = DiskTreeViewItem.GetDataName(path);
        
        var image = "Images/file.png";

        if (string.IsNullOrEmpty(name))
        {
            image = "Images/drive.png";
        }
        else if(new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
        {
            image = "Images/folder-closed.png";
        }

        return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}