using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Services;
using NUnit.Framework;

namespace EventBooking.Tests
{
	[TestFixture]
	public class CreateActivityTests
	{
		[Test]
		public void RedirectsToHomeAfterSuccessfulCreation()
		{
		    ISecurityService mockupSecurityService = new MockupSecurityService();
		    var controller = new ActivityController(mockupSecurityService);

			RedirectToRouteResult result = controller.Create();

			Assert.NotNull(result);
			Assert.AreEqual("Index", result.RouteValues["Action"]);
			Assert.AreEqual("Home", result.RouteValues["controller"]);
		}
	}
}