using System;
using System.Collections.Generic;
using System.Linq;
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
	public class ManageActivitySessionsTests
	{
		[SetUp]
		public void SetUp()
		{
			SecurityService = new MockupSecurityService { ReturnUser = new User { Team = new Team { Name = "The team" } } };
		}

		Session SetupRepoWithSession( Mock<ISessionRepository> mock, int activityId)
		{
			var activity = new Activity { Id = activityId, Name = "foo", Date = new DateTime( 2023, 11, 12 ) };
			var session = new Session
			{
				Id = 223,
				Activity = activity,
				FromTime = activity.Date.AddHours( 1 ).TimeOfDay,
				ToTime = activity.Date.AddHours( 2 ).TimeOfDay,
				VolunteersNeeded = 33
			};
			mock.Setup( r => r.GetSessionsForActivity( activityId ) ).Returns( () => new List<Session> { session } );
			return session;
		}

		[Test]
		public void WhenNavigatingToActivitySessions_RepositoryIsAskedToLoadSessionsForActivity()
		{
			var repoMock = new Mock<ISessionRepository>();
			var controller = CreateController(repoMock.Object);

			controller.Index(1);

			repoMock.Verify( r => r.GetSessionsForActivity(1), Times.Once());
			repoMock.Verify( r => r.GetSessionsForActivity(1337), Times.Never());
		}

		[Test]
		public void WhenNavigatingToActivitySessions_ViewShowsInfoAboutRequestedActivity()
		{
			var repoMock = new Mock<ISessionRepository>();
			var controller = CreateController(repoMock.Object);
			var session = SetupRepoWithSession(repoMock, 22);

			var result = controller.Index( 22 ) as ViewResult;

			Assert.IsNotNull( result, "No view" );
			var model = result.ViewData.Model as ActivitySessionsModel;
			Assert.IsNotNull( model, "No model" );
			Assert.IsNotNull( model.Activity, "No activity in model" );
			Assert.AreEqual( session.Activity.Name, model.Activity.Name );
			Assert.AreEqual( session.Id, model.Sessions.First().Id );
		}

		[Test]
		public void WhenNavigatingToActivitySessions_ViewModelContainsRequestedActivity()
		{
			var repoMock = new Mock<ISessionRepository>();
			var controller = CreateController(repoMock.Object);
			SetupRepoWithSession( repoMock, 1 );

			var result = controller.Index(1) as ViewResult;

			Assert.IsNotNull( result, "No view" );
			var model = result.ViewData.Model as ActivitySessionsModel;
			Assert.IsNotNull( model, "No model" );
			Assert.IsNotNull( model.Activity, "No activity in model" );
			Assert.AreEqual(1, model.Activity.Id );
		}

		[Test]
		public void WhenNavigatingToActivitySessions_ViewModelContainsSessions()
		{
			var repoMock = new Mock<ISessionRepository>();
			var controller = CreateController(repoMock.Object);
			SetupRepoWithSession( repoMock, 19 );

			var result = controller.Index(19) as ViewResult;

			Assert.IsNotNull(result, "No view" );
			var model = result.ViewData.Model as ActivitySessionsModel;
			Assert.IsNotNull( model, "No model" );
			Assert.AreEqual( 1, model.Sessions.Count, "No sessions in model" );
			Assert.AreEqual(19, model.Activity.Id );
		}

		[Test]
		public void CanAddSessionToActivity()
		{
			var repoMock = new Mock<ISessionRepository>();
            Activity activity = new Activity(){Id = 145};

			var controller = CreateController(repoMock.Object);
			repoMock.Setup(r => r.Save(145, null));
		    var activitySessionsModel = new ActivitySessionsModel(new ActivityModel(activity), new List<SessionModel>());


            RedirectToRouteResult result = (RedirectToRouteResult) controller.Save(activitySessionsModel);

			Assert.IsNotNull( result );
			Assert.AreEqual( "Index", result .RouteValues["Action"] );
			Assert.AreEqual( 145, result.RouteValues["activityId"] );
		}

		[Test]
		public void WhenNavigatingToNonExistentActivity_UserIsRedirectedToNotFound()
		{
			var repoMock = new Mock<ISessionRepository>();
			var controller = CreateController(repoMock.Object);

			var result = controller.Index( 133 ) as RedirectToRouteResult;

			Assert.IsNotNull( result );
			Assert.AreEqual( "NotFound", result.RouteValues["Action"] );
			Assert.AreEqual( 133, result.RouteValues["activityId"] );
		}

		protected MockupSecurityService SecurityService { get; set; }

		private SessionsController CreateController(ISessionRepository repository)
		{
			return new SessionsController( repository, new Mock<ISecurityService>().Object );
		}
	}
}