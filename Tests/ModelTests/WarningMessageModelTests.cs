using FakeItEasy;
using NUnit.Framework;
using Task10.Models;
using Task10.Services;

namespace ModelTests;

public class WarningMessageModelTests
{

    [Test]
    public void ConstructorTest()
    {
        var model = new WarningMessageModel();
    }


    [Test]
    public void CreateMessageTest()
    {
        var model = A.Fake<IMessageBoxService>();

        model.CreateMessageBox();
        
        A.CallTo(() => model.CreateMessageBox()).MustHaveHappened(1, Times.Exactly);
        
    }
}