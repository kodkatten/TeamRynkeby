using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Services;
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
        public void When_Nobody_tries_to_see_an_activity_then_Nobody_should_be_asked_to_go_through_the_security_checkpoint()
        {
            var activityOverview = GetActivityOverview<RedirectToRouteResult>();

            Assert.IsNotNull(activityOverview);
            Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
            Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
        }

        [Test]
        public void When_Somebody_wants_to_see_the_activity_list_they_are_allowed_to()
        {
            var activityOverview = GetActivityOverview<ViewResult>(loggedin: true);

            Assert.IsNotNull(activityOverview);
        }


        private static T GetActivityOverview<T>(bool loggedin = false) where T : class
        {
            var mockSecurityService = new MockupSecurityService {AcceptedEmail = string.Empty, AcceptedPassword = string.Empty};
            if (loggedin)
            {
                mockSecurityService.SignIn(string.Empty, string.Empty);
            }
            var activityController = new ActivityController(mockSecurityService);
            var activityOverview = activityController.Index() as T;
            return activityOverview;
        }
    }
}
