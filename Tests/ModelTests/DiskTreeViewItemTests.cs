using NUnit.Framework;
using Task10.Models;

namespace ModelTests;

[Apartment(ApartmentState.STA)]
public class DiskTreeViewItemTests
{
    [Test]
    public void ConstructorNullExceptionTest()
    {
        DiskTreeViewItem diskItem;
        Assert.Throws(typeof(ArgumentNullException), () => diskItem = new DiskTreeViewItem(null));
    }


    [Test]
    public void GetDataNameNullExceptionTest()
    {
        Assert.Throws(typeof(ArgumentNullException), () => DiskTreeViewItem.GetDataName(null));
    }
    
    
    [Test]
    public void GetDataNameTrueResultTest()
    {
        string testValue = "E:\\Project\\Doodle";
        string trueResult = "Doodle";
        var result = DiskTreeViewItem.GetDataName(testValue);

        Assert.AreEqual(trueResult, result);
    }
}
