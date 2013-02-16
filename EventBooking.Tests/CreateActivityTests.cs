using System;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Services;
using NUnit.Framework;

namespace EventBooking.Tests
{
	[SetUpFixture]
	internal class ClassSetup
	{

		[SetUp]
		public void Setup()
		{
			EventBookingMapper.SetupMappers();
		}
	}

	[TestFixture]
	public class CreateActivityTests
	{
		public static readonly DateTime Tomorrow = DateTime.Now.AddDays(1);
		public static readonly DateTime Yesterday = DateTime.Now.AddDays(-1);

		[Test]
		public void RedirectsToHomeAfterSuccessfulCreation()
		{
			var controller = new ActivityControllerShunt();

			var result = CreateValidActivity(controller) as RedirectToRouteResult;

			Assert.NotNull(result);
			Assert.AreEqual("Index", result.RouteValues["Action"]);
			Assert.AreEqual("Home", result.RouteValues["controller"]);
		}

		[Test]
		public void GivenDataForNewActivityIsPersisted()
		{
			var controller = new ActivityControllerShunt();

			CreateValidActivity(controller);

			Assert.IsNotNull( controller.CreatedActivity );
			Assert.AreEqual( "Name", controller.CreatedActivity.Name );
			Assert.AreEqual( Tomorrow, controller.CreatedActivity.Date );
			Assert.AreEqual( "Description", controller.CreatedActivity.Description );
			Assert.AreEqual( "Summary", controller.CreatedActivity.Summary );
			Assert.AreEqual( ActivityType.Preliminary, controller.CreatedActivity.Type );
		}

		private static ActionResult CreateValidActivity(ActivityControllerShunt controller)
		{
			return controller.Create(NewModel());
		}

		private static CreateActivityModel NewModel()
		{
			return new CreateActivityModel
				{
					Name = "Name",
					Date = Tomorrow,
					Description = "Description",
					Summary = "Summary",
					Type = ActivityType.Preliminary
				};
		}

		[Test,Ignore]
		public void StaysOnViewIfTryingToCreateActivity_ForYesterday()
		{
			var controller = new ActivityControllerShunt();
			var forYesterday = NewModel();
			forYesterday.Date = Yesterday;

			var result = controller.Create(forYesterday);

			Assert.NotNull(result);
			Assert.IsInstanceOf<ViewResult>(result);
		}
	}

	public class ActivityControllerShunt : ActivityController
	{
		public ActivityControllerShunt()
			: base( new MockupSecurityService(), null )
		{
			
		}

		protected override void StoreActivity( Activity activity )
		{
			CreatedActivity = activity;
		}

		public Activity CreatedActivity { get; set; }
	}
}