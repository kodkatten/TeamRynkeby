using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventBooking.Controllers;
using EventBooking.Services;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EventBooking.Tests
{
    /// <summary>
    /// Summary description for TeamControllerTests
    /// </summary>
    [TestFixture]
    public class TeamControllerTests
    {
        [Test]
        public void When_Somebody_wants_to_see_the_team_view_they_are_allowed_to()
        {
            var teamController = GetTeamController(loggedin: true);

            var viewResult = teamController.Details() as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void When_Nobody_wants_to_see_the_team_view_they_should_be_redirected_to_login_with_correct_redirect_url()
        {
            var activityOverview = GetTeamView<RedirectToRouteResult>(c => c.Details());

            Assert.IsNotNull(activityOverview);
            Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
            Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
        }

        private static TeamController GetTeamController(bool loggedin = false)
        {
            var mockSecurityService = new MockupSecurityService { AcceptedEmail = string.Empty, AcceptedPassword = string.Empty };
            if (loggedin)
            {
                mockSecurityService.SignIn(string.Empty, string.Empty);
            }
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var activityController = new TeamController(mockSecurityService)
            {
                ControllerContext = controllerContext,
                Url = new UrlHelper(controllerContext.RequestContext)
            };
            return activityController;
        }


        private static T GetTeamView<T>(Func<TeamController, ActionResult> navigate, bool loggedin = false) where T : class
        {
            var activityController = GetTeamController(loggedin);
            var activityOverview = navigate(activityController) as T;
            return activityOverview;
        }
    }
}
