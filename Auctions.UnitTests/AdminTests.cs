using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Auctions.Domain.Abstract;
using Auctions.Domain.Entities;
using Auctions.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Auctions.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Lots()
        {
            // Arrange - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            new Lot {LotID = 2, Name = "P2"},
            new Lot {LotID = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - create a controller
            AdminController target = new AdminController(mock.Object);
            // Action
            Lot[] result =
            ((IEnumerable<Lot>)target.Index().ViewData.Model).ToArray();
            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Lot()
        {
            // Arrange - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            new Lot {LotID = 2, Name = "P2"},
            new Lot {LotID = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Act
            Lot p1 = target.Edit(1).ViewData.Model as Lot;
            Lot p2 = target.Edit(2).ViewData.Model as Lot;
            Lot p3 = target.Edit(3).ViewData.Model as Lot;
            // Assert
            Assert.AreEqual(1, p1.LotID);
            Assert.AreEqual(2, p2.LotID);
            Assert.AreEqual(3, p3.LotID);
        }


        [TestMethod]
        public void Cannot_Edit_Nonexistent_Lot()
        {
            // Arrange - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            new Lot {LotID = 2, Name = "P2"},
            new Lot {LotID = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Act
            Lot result = (Lot)target.Edit(4).ViewData.Model;
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create a product
            Lot lot = new Lot { Name = "Test" };
            // Act - try to save the product
            ActionResult result = null;
            // Assert - check that the repository was called
            mock.Verify(m => m.SaveLot(lot));
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create a lot
            Lot lot = new Lot { Name = "Test" };
            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");
            // Act - try to save the lot
            ActionResult result = null;
            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveLot(It.IsAny<Lot>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Lots()
        {
            // Arrange - create a Lot
            Lot lotd = new Lot { LotID = 2, Name = "Test" };
            // Arrange - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            lotd,
            new Lot {LotID = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Act - delete the lot
            target.Delete(lotd.LotID);
            // Assert - ensure that the repository delete method was
            // called with the correct Lot
            mock.Verify(m => m.DeleteLot(lotd.LotID));
        }
    }
}
