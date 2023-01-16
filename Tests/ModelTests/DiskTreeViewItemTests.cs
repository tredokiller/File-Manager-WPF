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
    public void ConstructorTrueValuesTest()
    {
        string truePath = "E:\\Project\\Doodle";
        string trueName = "Doodle";
        DiskTreeViewItem diskItem = new DiskTreeViewItem(truePath);
        Assert.AreEqual(truePath, diskItem.DataPath);
        Assert.AreEqual(trueName, diskItem.DataName);
        
        
    }

    [Test]
    public void DataPathGetTest()
    {
        string truePath = "E:\\Project\\Doodle";
        
        
        DiskTreeViewItem diskItem = new DiskTreeViewItem(truePath);
        
        string testedPath = diskItem.DataPath;
        
        Assert.AreEqual(truePath, diskItem.DataPath);
    }
    
    
    [Test]
    public void DataNameGetTest()
    {
        string truePath = "E:\\Project\\Doodle";
        string trueName = "Doodle";
        
        
        DiskTreeViewItem diskItem = new DiskTreeViewItem(truePath);
        
        string testedPath = diskItem.DataPath;
        
        Assert.AreEqual(trueName, diskItem.DataName);
    }
    
    
    [Test]
    public void ConstructorStringEmptyValueTest()
    {
        string testPath = "C:\\";
        DiskTreeViewItem diskItem = new DiskTreeViewItem(testPath);
        Assert.AreEqual(testPath, diskItem.DataName);

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



    [Test]
    public void CalculateDiskSizeTest()
    {
        var drive = DriveInfo.GetDrives()[0];
        
        double trueSize = drive.TotalSize - drive.AvailableFreeSpace;
        double trueSizeString = trueSize / 1024 / 1024 / 1024;

        var t = new Thread(o =>
        {
            DiskTreeViewItem diskItem = new DiskTreeViewItem(drive.ToString());
            Assert.AreEqual(Math.Round(trueSizeString, 2) + "GB", diskItem.SizeOfFolder);
            
        });
        t.SetApartmentState(ApartmentState.STA);
        t.Start();


    }
}
