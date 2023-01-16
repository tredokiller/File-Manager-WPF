using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Moq;
using NUnit.Framework;
using Task10.Services;
using Task10.ViewModels;

namespace ViewModelTests;


[Apartment(ApartmentState.STA)]
public class MainViewModelTests
{
    [Test]
    [TestCase(null, null)]
    public void ConstructorExceptionsTest(ComboBox x, TreeView y)
    {
        
        MainViewModel viewModel;
        Assert.Throws(typeof(ArgumentNullException), () => viewModel = new MainViewModel(x, y));
    }



    [Test]
    public void CreateScannerButtonOnClickCommandTest()
    {
        var argumentX = new Mock<ComboBox>();
        var argumentY = new Mock<TreeView>();

        MainViewModel viewModel = new MainViewModel(argumentX.Object , argumentY.Object);

        Assert.AreEqual(typeof(RelayCommand) , viewModel.ScannerButtonOnClickCommand.GetType());
    }


    [Test]
    public void GetScannerButtonOnClickCommand()
    {
        var argumentX = new Mock<ComboBox>();
        var argumentY = new Mock<TreeView>();

        
        
        
        MainViewModel viewModel = new MainViewModel(argumentX.Object, argumentY.Object);


        var commandGetter = viewModel.ScannerButtonOnClickCommand;
        
        Assert.AreSame(viewModel.ScannerButtonOnClickCommand, commandGetter);
    }



    [Test]
    public void ScannerButtonOnClickCreateMessageTest()
    {
        var argumentX = new Mock<ComboBox>();
        var argumentY = new Mock<TreeView>();

        var mockService = new Mock<IMessageBoxService>();

        mockService.Setup(service => service.CreateMessageBox()).Verifiable();


        MainViewModel viewModel = new MainViewModel(argumentX.Object, argumentY.Object , mockService.Object);

        viewModel.ScannerButtonOnClickCommand.Execute(null);
        
        mockService.VerifyAll();
    }
}