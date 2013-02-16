using System;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;
using Moq;
using NUnit.Framework;

namespace EventBooking.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController;
        private Mock<ISecurityService> security;
        private Mock<IUserRepository> userRepository;

        [SetUp]
        public void SetUp()
        {
            this.security = new Mock<ISecurityService>();
            this.userRepository = new Mock<IUserRepository>();
        }

        [Test]
        public void SignUp_given_a_valid_model_creates_a_user_account()
        {
            this.security.Setup(s => s.CreateUserAndAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            this.security.Setup(s => s.SignIn(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var model = new SignUpModel();
            var view = this.userController.SignUp(model) as RedirectToRouteResult;
            Assert.IsNotNull(view);
            Assert.AreEqual("MyProfile", view.RouteValues["action"]);
        }

        [Test]
        public void SignUp_given_an_invalid_model_returns_user_to_same_page()
        {
            this.security.Setup(s => s.CreateUserAndAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            this.security.Setup(s => s.SignIn(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            InvalidateModel();

            var view = this.userController.SignUp(new SignUpModel()) as ViewResult;
            Assert.IsNotNull(view);
            Assert.AreEqual(string.Empty, view.ViewName); // viewname is empty when returning the default view
        }

        private void InvalidateModel()
        {
            this.userController.ModelState.AddModelError("key", "there's an error");
        }

        [Test]
        public void Get_myprofile_returns_view()
        {
            var view = this.userController.MyProfile() as ViewResult;
            Assert.IsNotNull(view);
            Assert.IsAssignableFrom<MyProfileModel>(view.Model);
        }

        [Test]
        public void Post_valid_profile_redirect_user_to_dashboard()
        {
            var model = new MyProfileModel();
            var view = this.userController.MyProfile(model) as RedirectToRouteResult;
            Assert.IsNotNull(view);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [Test]
        public void Post_invalid_profile_returns_user_to_same_page()
        {
            InvalidateModel();

            var view = this.userController.MyProfile(new MyProfileModel()) as ViewResult;
            Assert.IsNotNull(view);
            Assert.AreEqual(string.Empty, view.ViewName); // viewname is empty when returning the default view
        }

        [Test]
        public void Valid_profile_is_saved()
        {
            var view = this.userController.MyProfile(new MyProfileModel()) as RedirectToRouteResult;
            Assert.IsNotNull(view);

            userRepository.Verify(r => r.Save(It.IsAny<User>()));
        }
    }
}
