using System.Globalization;
using FakeItEasy;
using NUnit.Framework;
using Task10.Models;

namespace ModelTests;

public class HeaderToImageConvertorTests
{
    

    [Test]
    public void ConvertNullExceptionTest()
    {
        var convertor = new HeaderToImageConvertor();



        var type = A.Fake<Type>();
        var cultInfo = A.Fake<CultureInfo>();


        Assert.Throws(typeof(ArgumentNullException), () => convertor.Convert(null, type, null, cultInfo));
    }


    [Test]
    public void ConvertBackExceptionTest()
    {
        var convertor = new HeaderToImageConvertor();


        Assert.Throws(typeof(NotImplementedException), () => convertor.ConvertBack(null, null, null, null));
    }


    [Test]
    public void GetInstanceTest()
    { 
        var convertor = new HeaderToImageConvertor();

        var instance = HeaderToImageConvertor.Instance;
        
        Assert.AreEqual(typeof(HeaderToImageConvertor), instance.GetType());
    }


    [Test]
    public void ConstructorTest()
    {
        var convertor = new HeaderToImageConvertor();
    }
    
    
}