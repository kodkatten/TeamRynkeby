using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventBooking.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void Get_myprofile_returns_view()
        {
            var view = new UserController().MyProfile() as ViewResult;
            Assert.IsNotNull(view);
            Assert.IsInstanceOfType(view.Model, typeof(MyProfileModel));
        }

        [TestMethod]
        public void SignUp_given_a_valid_model_creates_a_user_account()
        {
        }
    }
}
