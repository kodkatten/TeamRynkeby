using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventBooking.Tests
{
    [TestClass]
    public class SecurityControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var controller = new SecurityController();

            var result = controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(LoginModel));

        }
    }
}
