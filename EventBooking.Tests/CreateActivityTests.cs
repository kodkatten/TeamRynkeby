using System.Web.Mvc;
using EventBooking.Controllers;
using NUnit.Framework;

namespace EventBooking.Tests
{
	[TestFixture]
	public class CreateActivityTests
	{
		[Test]
		public void RedirectsToHomeAfterSuccessfulCreation()
		{
			var controller = new ActivityController();

			RedirectToRouteResult result = controller.Create();

			Assert.NotNull(result);
			Assert.AreEqual("Index", result.RouteValues["Action"]);
			Assert.AreEqual("Home", result.RouteValues["controller"]);
		}
	}
}