using System;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using Moq;
using NUnit.Framework;

namespace EventBooking.Tests
{
	[TestFixture]
	public class ManageActivitySessionsTests
	{
		[SetUp]
		public void SetUp()
		{
			SecurityService = new MockupSecurityService { ReturnUser = new User { Team = new Team { Name = "The team" } } };
		}

		[Test]
		public void WhenNavigatingToActivitySessions_RepositoryIsAskedToLoadSessionsForActivity()
		{
			var repoMock = new Mock<SessionRepository>();
			var controller = CreateController(repoMock.Object);

			controller.Index(1);

			repoMock.Verify( r => r.GetSessionsForActivity(1), Times.Once());
			repoMock.Verify( r => r.GetSessionsForActivity(1337), Times.Never());
		}

		[Test]
		public void WhenNavigatingToActivitySessions_ViewModelContainsRequestedActivity()
		{
			var repoMock = new Mock<SessionRepository>();
			var controller = CreateController(repoMock.Object);

			var result = controller.Index(1) as ViewResult;
			var model = result.ViewData.Model as ActivitySessionsModel;

			Assert.IsNotNull(model, "No model" );
			Assert.IsNotNull(model.Activity, "No activity in model" );
			Assert.AreEqual("1", model.Activity.Id );
		}

		protected MockupSecurityService SecurityService { get; set; }

		private SessionsController CreateController(SessionRepository repository)
		{
			return new SessionsController( repository );
		}
	}
}