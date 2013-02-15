using System;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventBooking.Tests
{
    [TestClass]
    public class SecurityControllerTests
    {
        private SecurityController _controller;

        private ViewResult GimmeIndexView()
        {
            _controller = _controller ?? new SecurityController();
            return _controller.Checkpoint() as ViewResult;
        }


        [TestMethod]
        public void Index_Returns_View()
        {
            var viewResult = GimmeIndexView();

            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(LoginModel));
        }


        [TestMethod]
        public void Given_a_nobody_When_nobody_logs_in_As_somebody_Then_somebody_gets_redirected_to_Team_Index()
        {
            var controller = new SecurityController();
            var model = new LoginModel() { ElectronicMailAddress = "a@b.c", Password = "you know it" };

            var result = controller.LogIn(model);
        }

        [TestMethod]
        public void Given_a_nobody_When_nobody_tries_to_log_As_somebody_but_doesnt_exist_Then_return_checkpoint_view_responding_vaguely() // for sec. reasons (checkpoint!).
        {
            var controller = new SecurityController();
            var model = new LoginModel() { ElectronicMailAddress = "a@b.c", Password = "you don't know it" };

            var result = controller.LogIn(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Checkpoint", result.ViewName);
            var resultingModel = result.Model as LoginModel;
            Assert.IsNotNull(resultingModel);
            Assert.AreEqual("E-postadress eller lösenord är felaktigt", resultingModel.ErrorMessage);
        }
    }
}
