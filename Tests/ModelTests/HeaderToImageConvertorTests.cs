using System.Globalization;
using FakeItEasy;
using NUnit.Framework;
using Task10.Models;

namespace ModelTests;

public class HeaderToImageConvertorTests
{


    private string _filePath = "Test.txt";
    private string _fullFilePath = "";
    
    [SetUp]
    public void Default()
    {
    }

    [Test]
    public void ConvertNullExceptionTest()
    {
        var convertor = new HeaderToImageConvertor();



        var type = A.Fake<Type>();
        var cultInfo = A.Fake<CultureInfo>();


        Assert.Throws(typeof(ArgumentNullException), () => convertor.Convert(null, type, null, cultInfo));
    }
    

    [TearDown]
    public void Exit()
    {
    }
    
    
    
    
}