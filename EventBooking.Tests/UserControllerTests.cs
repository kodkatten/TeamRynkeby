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
		[SetUp]
		public void SetUp()
		{
			security = new MockupSecurityService();
			userRepository = new Mock<IUserRepository>();
			teamRepository = new Mock<ITeamRepository>();
			userController = new UserController(security, userRepository.Object, teamRepository.Object);
		}

		private UserController userController;
		private MockupSecurityService security;
		private Mock<IUserRepository> userRepository;
		private Mock<ITeamRepository> teamRepository;

		private void InvalidateModel()
		{
			userController.ModelState.AddModelError("key", "there's an error");
		}

		[Test]
		public void Get_myprofile_returns_view()
		{
			var view = userController.MyProfile() as ViewResult;
			Assert.IsNotNull(view);
			Assert.IsAssignableFrom<MyProfileModel>(view.Model);
		}

		[Test]
		public void Post_invalid_profile_returns_user_to_same_page()
		{
			InvalidateModel();

			var view = userController.MyProfile(new MyProfileModel()) as ViewResult;
			Assert.IsNotNull(view);
			Assert.AreEqual(string.Empty, view.ViewName); // viewname is empty when returning the default view
		}

		[Test]
		public void Post_valid_profile_redirect_user_to_dashboard()
		{
			security.ReturnUser = new User();
			var model = new MyProfileModel();
			var view = userController.MyProfile(model) as RedirectToRouteResult;
			Assert.IsNotNull(view);
			Assert.AreEqual("Index", view.RouteValues["action"]);
		}

		[Test]
		public void SignUp_given_a_valid_model_creates_a_user_account()
		{
			var model = new SignUpModel();
			var view = userController.SignUp(model) as RedirectToRouteResult;
			Assert.IsNotNull(view);
			Assert.AreEqual("MyProfile", view.RouteValues["action"]);
		}

		[Test]
		public void SignUp_given_an_invalid_model_returns_user_to_same_page()
		{
			InvalidateModel();

			var view = userController.SignUp(new SignUpModel()) as ViewResult;
			Assert.IsNotNull(view);
			Assert.AreEqual(string.Empty, view.ViewName); // viewname is empty when returning the default view
		}

		[Test]
		public void Valid_profile_is_saved()
		{
			security.ReturnUser = new User();
			var view = userController.MyProfile(new MyProfileModel()) as RedirectToRouteResult;
			Assert.IsNotNull(view);

			userRepository.Verify(r => r.Save(It.IsAny<User>()));
		}
	}
}