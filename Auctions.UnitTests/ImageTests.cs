using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Auctions.Domain.Abstract;
using Auctions.Domain.Entities;
using Auctions.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace Auctions.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Arrange - create a Product with image data
            Lot prod = new Lot
            {
                LotID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                Preview = "image/png"
            };
            // Arrange - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            prod,
            new Lot {LotID = 3, Name = "P3"}
            }.AsQueryable());
            // Arrange - create the controller
            LotController target = new LotController(mock.Object);
            // Act - call the GetImage action method
            ActionResult result = target.GetImage(2);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.Preview, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Arrange - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            new Lot {LotID = 2, Name = "P2"}
            }.AsQueryable());
            // Arrange - create the controller
            LotController target = new LotController(mock.Object);
            // Act - call the GetImage action method
            ActionResult result = target.GetImage(100);
            // Assert
            Assert.IsNull(result);
        }
    }
}