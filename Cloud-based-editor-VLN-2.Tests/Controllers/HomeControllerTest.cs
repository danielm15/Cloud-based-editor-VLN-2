using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2;
using Cloud_based_editor_VLN_2.Controllers;

namespace Cloud_based_editor_VLN_2.Tests.Controllers {
    [TestClass]
    public class HomeControllerTest {
        [TestMethod]
        public void Index() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Support() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Support() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
