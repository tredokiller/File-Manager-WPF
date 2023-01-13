using System.Windows.Controls;
using NUnit.Framework;
using Task10.ViewModels;

namespace ViewModelTests;

public class MainViewModelTests
{
    [Test]
    [TestCase(null, null)]
    public void ConstructorExceptionsTest(ComboBox x, TreeView y)
    {
        
        MainViewModel viewModel;
        Assert.Throws(typeof(ArgumentNullException), () => viewModel = new MainViewModel(ref x, ref y));
    }
}