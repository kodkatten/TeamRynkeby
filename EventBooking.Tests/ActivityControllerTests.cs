using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
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
        private readonly Mock<IEmailService> _emailService = new Mock<IEmailService>();
        private readonly Mock<HttpRequestBase> _request = new Mock<HttpRequestBase>();
        private readonly Mock<HttpContextBase> _mockHttpContext = new Mock<HttpContextBase>();
        private readonly Mock<IActivityItemRepository> _itemRepository = new Mock<IActivityItemRepository>();
        private readonly Mock<ISecurityService> _securityService = new Mock<ISecurityService>();
        private readonly Mock<IActivityRepository> _activityRepository = new Mock<IActivityRepository>();
        private ControllerContext _controllerContext;

        [SetUp]
        public void Setup()
        {
            _emailService.Setup(mock => mock.SendMail(1, EmailService.EmailType.InfoActivity, "Test"));
            _request.Setup(r => r.HttpMethod).Returns("GET");
            _mockHttpContext.Setup(c => c.Request).Returns(_request.Object);
            _itemRepository.Setup(r => r.GetTemplates()).Returns(Enumerable.Empty<ActivityItemTemplate>().AsQueryable);
            _securityService.Setup(mock => mock.GetCurrentUser())
                           .Returns(new User
                           {
                               Name = "Ninja",
                               Id = 1,
                               Cellphone = "123456",
                               Team = new Team
                               {
                                   Id = 1,
                                   Name = "Stockholm"
                               }

                           });

        }

        [Test]
        public void CreateEvents()
        {
            //arrange
            var controllerContext = new ControllerContext(_mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var sessions = new List<SessionModel>
                {
                    new SessionModel
                        {
                            FromTime = new TimeSpan(12, 00, 00),
                            ToTime = new TimeSpan(14, 00, 00),
                            VolunteersNeeded = 3
                        }
                };

            var createActivityModel = new CreateActivityModel
                {
                    Date = new DateTime().AddDays(1),
                    Description = "Description",
                    Name = "Name",
                    Summary = "Summary",
                    Sessions = sessions

                };

            var activityController = new ActivityController(_securityService.Object, _activityRepository.Object, _itemRepository.Object, null, _emailService.Object, null)
            {
                ControllerContext = controllerContext,
                Url = new UrlHelper(controllerContext.RequestContext)
            };

            //act
            activityController.Create(createActivityModel);


            //assert
            _activityRepository.Verify(y => y.Add(It.IsAny<Activity>()));

        }

        [Test]
        [Ignore("Using FluentSecurity now instead")]
        public void When_Nobody_tries_to_create_an_activity_then_Nobody_should_be_asked_to_go_through_the_security_checkpoint()
        {
            var activityOverview = GetActivityView<RedirectToRouteResult>(c => c.Create());

            Assert.IsNotNull(activityOverview);
            Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
            Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
        }

        [Test]
        [Ignore("Unsure if this is true. The way this has been implemented a nobody will see the public information of an activity.")]
        public void When_Nobody_tries_to_see_an_activity_then_Nobody_should_be_asked_to_go_through_the_security_checkpoint()
        {
            var activityOverview = GetActivityView<RedirectToRouteResult>(c => c.Details(1));

            Assert.IsNotNull(activityOverview);
            Assert.AreEqual("Security", activityOverview.RouteValues["Controller"]);
            Assert.AreEqual("Checkpoint", activityOverview.RouteValues["Action"]);
        }

        [Test]
        public void When_Somebody_wants_to_see_the_activity_list_they_are_allowed_to()
        {
            var activityOverview = GetActivityView<ViewResult>(c => c.Create(), loggedin: true);

            Assert.IsNotNull(activityOverview);
        }

        private static ActivityController GetActivityController(ISecurityService securityService)
        {

            var emailService = new Mock<IEmailService>();
            emailService.Setup(mock => mock.SendMail(1, EmailService.EmailType.InfoActivity, "Test"));

            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);

            var itemRepository = new Mock<IActivityItemRepository>();
            itemRepository.Setup(r => r.GetTemplates()).Returns(Enumerable.Empty<ActivityItemTemplate>().AsQueryable);


            var activityRepository = new Mock<IActivityRepository>();
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var activityController = new ActivityController(securityService, activityRepository.Object, itemRepository.Object, null, emailService.Object, null)
                {
                    ControllerContext = controllerContext,
                    Url = new UrlHelper(controllerContext.RequestContext)
                };

            return activityController;
        }


        private static T GetActivityView<T>(Func<ActivityController, ActionResult> navigate, bool loggedin = false) where T : class
        {
            var mockSecurityService = new MockupSecurityService { AcceptedEmail = string.Empty, AcceptedPassword = string.Empty };
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
