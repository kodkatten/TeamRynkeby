using System;
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
    /// Summary description for ActivityControllerTests
    /// </summary>
    [TestFixture]
    public class ActivityControllerTests
    {
  

        [Test]
        public void When_Nobody_tries_to_create_an_activity_then_Nobody_should_be_asked_to_go_through_the_security_checkpoint()
        {
            var activityOverview = GetActivityView<RedirectToRouteResult>(c=>c.Create());

            Assert.IsNotNull(activityOverview);
            Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
            Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
        }

        [Test]
        public void When_Nobody_tries_to_see_an_activity_then_Nobody_should_be_asked_to_go_through_the_security_checkpoint()
        {
            var activityOverview = GetActivityView<RedirectToRouteResult>(c=>c.Details(1));

            Assert.IsNotNull(activityOverview);
            Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
            Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
        }

        [Test]
        public void When_Somebody_wants_to_see_the_activity_list_they_are_allowed_to()
        {
            var activityOverview = GetActivityView<ViewResult>(c=>c.Create(), loggedin: true);

            Assert.IsNotNull(activityOverview);
        }

        private static ActivityController GetActivityController(ISecurityService securityService)
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var activityController = new ActivityController(securityService, null)
                {
                    ControllerContext = controllerContext,
                    Url = new UrlHelper(controllerContext.RequestContext)
                };
            return activityController;
        }


        private static T GetActivityView<T>(Func<ActivityController, ActionResult> navigate, bool loggedin = false) where T : class
        {
            var mockSecurityService = new MockupSecurityService {AcceptedEmail = string.Empty, AcceptedPassword = string.Empty};
            if (loggedin)
            {
                mockSecurityService.SignIn(string.Empty, string.Empty);
            }
            var activityController = GetActivityController(mockSecurityService);
            var activityOverview = navigate(activityController) as T;
            return activityOverview;
        }
    }
}
