using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Services;
using Moq;
using NUnit.Framework;
namespace EventBooking.Tests
{
    public class EditActivityTest
    {
        private readonly Mock<IEmailService> _emailService = new Mock<IEmailService>();
        private readonly Mock<HttpRequestBase> _request = new Mock<HttpRequestBase>();
        private readonly Mock<HttpContextBase> _mockHttpContext = new Mock<HttpContextBase>();
        private readonly Mock<IActivityItemRepository> _itemRepository = new Mock<IActivityItemRepository>();
        private readonly Mock<ISecurityService> _securityService = new Mock<ISecurityService>();
        private readonly Mock<IActivityRepository> _activityRepository = new Mock<IActivityRepository>();
        private readonly Mock<ISessionRepository> _sessionRepository = new Mock<ISessionRepository>(); 
        private ControllerContext _controllerContext;

        [SetUp]
        public void SetUp()
        {
            //skapa en aktivitet
            var sessionList = SetSessions();
            _emailService.Setup(x => x.SendMail(1, EmailService.EmailType.NewActivity, "test"));
            _request.Setup(r => r.HttpMethod).Returns("GET");
            _mockHttpContext.Setup(c => c.Request).Returns(_request.Object);
            _itemRepository.Setup(r => r.GetTemplates()).Returns(Enumerable.Empty<ActivityItemTemplate>().AsQueryable);
            _sessionRepository.Setup(s => s.GetSessionsForActivity(It.IsAny<int>())).Returns(sessionList);

            var activity = new Activity
            {
                Id = 1,
                Name = "Ninja",
                Description = "Description",
                Summary = "Summary",
                Type = ActivityType.Sponsor,
                Date = DateTime.Now.AddDays(1),
                Sessions = SetSessions(),
                Coordinator = SetCoordinator(),
                OrganizingTeam = SetTeam()
            };

            _activityRepository.Setup(x => x.GetActivityById(1)).Returns(activity);
        }

        [Test]
        public void Edit_Text_For_An_Event()
        {
           
            var controller = new ActivityController(_securityService.Object, _activityRepository.Object,
                                                    _itemRepository.Object, null, _emailService.Object, _sessionRepository.Object);

            var result = controller.Edit(1) as ViewResult;

            var model = (EditActivityViewModel)result.ViewData.Model;

            model.Activity.Description = "Description Updated";
            model.Activity.Name = "Name Updated";
            model.Activity.Summary = "Summary Updated";
            model.Sessions = SetSessions();
            
            controller.EditActivity(model);

            _activityRepository.Verify(x => x.UpdateActivity(model.Activity));
            

        }

        private static List<Session> SetSessions()
        {
            var sessions = new List<Session> 
            {
                new Session
                    {
                        FromTime = new TimeSpan(12, 00, 00),
                        ToTime = new TimeSpan(14, 00, 00),
                        VolunteersNeeded = 5,
                        Volunteers = new Collection<User>()
                    },

                new Session
                    {
                        FromTime = new TimeSpan(14, 00, 00),
                        ToTime = new TimeSpan(16, 00, 00),
                        VolunteersNeeded = 5,
                        Volunteers = new Collection<User>()
                    }
            };

            return sessions;
        }

        private static User SetCoordinator()
        {
            var user = new User
                {
                    Id = 1,
                    Name = "Ninja Ninjasson",
                    Email = "Ninja@tretton37.com",
                    Cellphone = "1234567"
                };
            return user;
        }

        private static Team SetTeam()
        {
            var team = new Team
                {
                    Id = 1,
                    Name = "Team Ninja"
                };
            return team;
        }

    }
}
