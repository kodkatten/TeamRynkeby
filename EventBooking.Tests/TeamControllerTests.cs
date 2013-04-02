using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EventBooking.Tests
{
	/// <summary>
	///     Summary description for TeamControllerTests
	/// </summary>
	[TestFixture]
	public class TeamControllerTests
	{
		private static readonly Team _leetTeam;

		static TeamControllerTests()
		{
			_leetTeam = new Team {Id = 1337, Activities = new Activity[0]};
		}

		private static TeamController GetTeamController(User user = null)
		{
			var mockSecurityService = new MockupSecurityService
				{
					AcceptedEmail = string.Empty,
					AcceptedPassword = string.Empty,
					ReturnUser = user
				};
			var request = new Mock<HttpRequestBase>();
			request.Setup(r => r.HttpMethod).Returns("GET");
			var mockHttpContext = new Mock<HttpContextBase>();
			mockHttpContext.Setup(c => c.Request).Returns(request.Object);
			var teamRepository = new Mock<ITeamRepository>();
			teamRepository.Setup(r => r.Get(1337)).Returns(_leetTeam);
			var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(),
			                                              new Mock<ControllerBase>().Object);
			var activityController = new TeamController(mockSecurityService, teamRepository.Object)
				{
					ControllerContext = controllerContext,
					Url = new UrlHelper(controllerContext.RequestContext)
				};
			return activityController;
		}


		private static T GetTeamView<T>(Func<TeamController, ActionResult> navigate, User user = null) where T : class
		{
			TeamController activityController = GetTeamController(user);
			var activityOverview = navigate(activityController) as T;
			return activityOverview;
		}

		[Test]
		public void When_Nobody_wants_to_see_the_team_view_they_should_be_redirected_to_login_with_correct_redirect_url()
		{
			var activityOverview = GetTeamView<RedirectToRouteResult>(c => c.Details());

			Assert.IsNotNull(activityOverview);
			Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
			Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
		}

		[Test]
		public void When_Somebody_wants_to_see_the_team_view_but_not_in_any_team_should_redirect_to_profile()
		{
			var user = new User
				{
					Created = DateTime.UtcNow,
					Name = "maaaah",
					Team = null
				};

			var viewResult = GetTeamView<RedirectToRouteResult>(c => c.Details(), user);

			Assert.IsNotNull(viewResult);
			Assert.AreEqual("User", viewResult.RouteValues["Controller"]);
			Assert.AreEqual("MyProfile", viewResult.RouteValues["Action"]);
		}

		[Test]
		public void When_Somebody_wants_to_see_the_team_view_they_are_allowed_to()
		{
			TeamController teamController = GetTeamController(new User {Team = _leetTeam});

			var viewResult = teamController.Details() as ViewResult;

			Assert.IsNotNull(viewResult);
		}

        [Test]
        public void When_No_date_is_specified_Model_should_have_current_date()
        {
            var teamController = GetTeamController(new User { Team = _leetTeam });

            var viewResult = teamController.Details() as ViewResult;
            
            var expectedDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month,1);
            Assert.IsNotNull(viewResult);
            var teamModel = viewResult.Model as TeamActivitiesModel;
            Assert.IsNotNull(teamModel, "No team model");
            Assert.AreEqual(expectedDate, teamModel.StartDate);
        }

        [Test]
        public void When_start_date_is_specified_Model_should_have_specified_date()
        {
            var user = new User {Team = _leetTeam};
            var teamController = GetTeamController(user);

            var currentDate = DateTime.UtcNow.AddMonths(2);
            var expectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            var viewResult = teamController.DetailsWithDate(expectedDate) as ViewResult;
            
            Assert.IsNotNull(viewResult);
            var teamModel = viewResult.Model as TeamActivitiesModel;
            Assert.IsNotNull(teamModel, "No team model");
            Assert.AreEqual(expectedDate, teamModel.StartDate);
        }

        [Test]
        public void When_doing_previous_on_model_Model_should_get_previous_month()
        {
            var user = new User {Team = _leetTeam};
            var teamController = GetTeamController(user);

            var currentDate = DateTime.UtcNow.AddMonths(2);
            var expectedDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
            var viewResult = teamController.Previous(currentDate) as ViewResult;
            
            Assert.IsNotNull(viewResult);
            var teamModel = viewResult.Model as TeamActivitiesModel;
            Assert.IsNotNull(teamModel, "No team model");
            Assert.AreEqual(expectedDate, teamModel.StartDate);
        }

        [Test]
        public void When_doing_next_on_model_Model_should_get_next_month()
        {
            var user = new User {Team = _leetTeam};
            var teamController = GetTeamController(user);

            var currentDate = DateTime.UtcNow.AddMonths(2);
            var expectedDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
            var viewResult = teamController.Next(currentDate) as ViewResult;
            
            Assert.IsNotNull(viewResult);
            var teamModel = viewResult.Model as TeamActivitiesModel;
            Assert.IsNotNull(teamModel, "No team model");
            Assert.AreEqual(expectedDate, teamModel.StartDate);
        }
	}
}