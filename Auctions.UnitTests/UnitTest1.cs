using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Auctions.Domain.Abstract;
using Auctions.Domain.Entities;
using Auctions.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using Auctions.WebUI;
using System.Web.Mvc;
using Auctions.WebUI.Models;
using System;
using Auctions.WebUI.HtmlHelpers;

namespace Auctions.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            new Lot {LotID = 2, Name = "P2"},
            new Lot {LotID = 3, Name = "P3"},
            new Lot {LotID = 4, Name = "P4"},
            new Lot {LotID = 5, Name = "P5"}
            }.AsQueryable());
            // create a controller and make the page size 3 items
            LotController controller = new LotController(mock.Object);
            controller.PageSize = 3;
            // Act
            LotsListViewModel result
            = (LotsListViewModel)controller.List(null, 2).Model;
            // Assert
            Lot[] prodArray = result.Lots.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // Assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
            + @"<a class=""selected"" href=""Page2"">2</a>"
            + @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1"},
            new Lot {LotID = 2, Name = "P2"},
            new Lot {LotID = 3, Name = "P3"},
            new Lot {LotID = 4, Name = "P4"},
            new Lot {LotID = 5, Name = "P5"}
            }.AsQueryable());
            // Arrange
            LotController controller = new LotController(mock.Object);
            controller.PageSize = 3;
            // Act
            LotsListViewModel result = (LotsListViewModel)controller.List(null, 2).Model;
            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            // - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1", Category = "Cat1"},
            new Lot {LotID = 2, Name = "P2", Category = "Cat2"},
            new Lot {LotID = 3, Name = "P3", Category = "Cat1"},
            new Lot {LotID = 4, Name = "P4", Category = "Cat2"},
            new Lot {LotID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            // Arrange - create a controller and make the page size 3 items
            LotController controller = new LotController(mock.Object);
            controller.PageSize = 3;
            // Action
            Lot[] result = ((LotsListViewModel)controller.List("Cat2", 1).Model)
            .Lots.ToArray();
            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            // - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1", Category = "Apples"},
            new Lot {LotID = 2, Name = "P2", Category = "Apples"},
            new Lot {LotID = 3, Name = "P3", Category = "Plums"},
            new Lot {LotID = 4, Name = "P4", Category = "Oranges"},
            }.AsQueryable());
            // Arrange - create the controller
            NavController target = new NavController(mock.Object);
            // Act = get the set of categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();
            // Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange
            // - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1", Category = "Apples"},
            new Lot {LotID = 4, Name = "P2", Category = "Oranges"},
            }.AsQueryable());
            // Arrange - create the controller
            NavController target = new NavController(mock.Object);
            // Arrange - define the category to selected
            string categoryToSelect = "Apples";
            // Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;
            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            // - create the mock repository
            Mock<ILotRepository> mock = new Mock<ILotRepository>();
            mock.Setup(m => m.Lots).Returns(new Lot[] {
            new Lot {LotID = 1, Name = "P1", Category = "Cat1"},
            new Lot {LotID = 2, Name = "P2", Category = "Cat2"},
            new Lot {LotID = 3, Name = "P3", Category = "Cat1"},
            new Lot {LotID = 4, Name = "P4", Category = "Cat2"},
            new Lot {LotID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            // Arrange - create a controller and make the page size 3 items
            LotController target = new LotController(mock.Object);
            target.PageSize = 3;
            // Action - test the product counts for different categories
            int res1 = ((LotsListViewModel)target
            .List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((LotsListViewModel)target
            .List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((LotsListViewModel)target
            .List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((LotsListViewModel)target
            .List(null).Model).PagingInfo.TotalItems;
            // Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
