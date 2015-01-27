using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Services;
using Moq;
using NUnit.Framework;

namespace EventBooking.Tests
{
    [TestFixture]
    public class CreateActivityTestsII
    {
        [SetUp]
        public void SetUp()
        {
            SecurityService = new MockupSecurityService { ReturnUser = new User { Team = new Team { Name = "The team" } } };
            EmailServices = new Moq.Mock<IEmailService>();
            userRepository = new Mock<IUserRepository>();
            teamRepository = new Mock<ITeamRepository>();
            userController = new UserController(security, userRepository.Object, teamRepository.Object);

        }

        private UserController userController;
        private MockupSecurityService security;
        private Mock<IUserRepository> userRepository;
        private Mock<ITeamRepository> teamRepository;

        public static readonly DateTime Tomorrow = DateTime.Now.AddDays(1);
        public static readonly DateTime Yesterday = DateTime.Now.AddDays(-1);

        protected MockupSecurityService SecurityService { get; set; }
        protected Mock<IEmailService> EmailServices { get; set; }

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
                    Information = "Description",
                    Type = ActivityType.Preliminärt
                };
        }

        [Test]
        public void GivenDataForNewActivityIsPersisted()
        {
            var controller = CreateController();

            CreateValidActivity(controller);

            Assert.IsNotNull(controller.CreatedActivity);
            Assert.AreEqual("Name", controller.CreatedActivity.Name);
            Assert.AreEqual(Tomorrow, controller.CreatedActivity.Date);
            Assert.AreEqual("Description", controller.CreatedActivity.Description);
            Assert.AreEqual("Summary", controller.CreatedActivity.Summary);
            Assert.AreEqual(ActivityType.Preliminärt, controller.CreatedActivity.Type);
            Assert.AreEqual(SecurityService.ReturnUser.Team.Name, controller.CreatedActivity.OrganizingTeam.Name);
        }

        [Test]
        public void RedirectsToActivityDetailsAfterSuccessfulCreation()
        {
            var controller = CreateController();

            var result = CreateValidActivity(controller) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.AreEqual("Details", result.RouteValues["Action"]);
            Assert.AreEqual("Activity", result.RouteValues["controller"]);
            Assert.IsNotNull(result.RouteValues["Id"]);
        }

        [Test]
        public void StaysOnViewIfTryingToCreateActivity_ForYesterday()
        {
            var controller = this.CreateController();

            CreateActivityModel forYesterday = NewModel();
            forYesterday.Date = Yesterday;

            ActionResult result = controller.Create(forYesterday);

            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        

        private ActivityControllerShunt CreateController()
        {
            return new ActivityControllerShunt(new ActivityRepositoryShunt(), SecurityService, null, EmailServices.Object);
        }
    }

    public class ActivityRepositoryShunt : ActivityRepository
    {
        public ActivityRepositoryShunt()
            : base(null)
        {
        }

        public override Activity GetActivityById(int id)
        {
            return new Activity();
        }
    }

    public class ActivityControllerShunt : ActivityController
    {
        public ActivityControllerShunt(ActivityRepository activityRepository, ISecurityService securityService, ITeamRepository teams, IEmailService emailService)
            : base(securityService, activityRepository, teams, emailService,null)
        {
        }

        public Activity CreatedActivity { get; set; }

        protected override void StoreActivity(Activity activity)
        {
            CreatedActivity = activity;
        }

    }
}