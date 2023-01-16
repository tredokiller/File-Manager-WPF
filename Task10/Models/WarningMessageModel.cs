using System.Windows;
using Task10.Services;

namespace Task10.Models;

public class WarningMessageModel : IMessageBoxService
{
    public void CreateMessageBox()
    {
        CreateWarningButton();
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