using System;
using System.Collections.Generic;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Services;
using NSubstitute;
using NUnit.Framework;
namespace EventBooking.Tests
{
    [TestFixture]
    public class CreateActivityTest
    {
        private ISecurityService _securityService;
        private IActivityRepository _activityRepository;
        private IUserRepository _userRepository;
        private ITeamRepository _teamRepository;
        private IEmailService _emailService;
        private ISessionRepository _sessionRepository;
        private ActivityController _activityController;
        private CreateActivityModel createActivityModel;

        public CreateActivityTest(UserController userController, CreateActivityModel createActivityModel)
        {

            this.createActivityModel = createActivityModel;
        }

        [SetUp]
        public void Setup()
        {
            _activityRepository = Substitute.For<IActivityRepository>();
            
            _teamRepository = Substitute.For<ITeamRepository>();
            _emailService = Substitute.For<IEmailService>();
            _securityService = Substitute.For<ISecurityService>();
            _sessionRepository = Substitute.For<ISessionRepository>();

            _securityService.GetCurrentUser().Team.Returns(SetupTeam());
            _securityService.GetCurrentUser().Returns(SetUpUser());

            _activityController = new ActivityController(_securityService, _activityRepository, _teamRepository,
                _emailService, _sessionRepository);
        }

        [Test]
        public void CreateActivity()
        {
            //Arrange
            var createActivityModel = new CreateActivityModel
            {
                Name = "Unit testing of create activity",
                Information = "This is the ultimate test for this controllers action method",
                Type = ActivityType.Sponsor,
                Date = new DateTime().AddDays(3),
                Sessions = GetSessionModels()
            };


            //Act
            var result = _activityController.Create(createActivityModel);

            //Assert

        }

        private List<SessionModel> GetSessionModels()
        {
            var sessionModel = new List<SessionModel>
            {
                new SessionModel
                {
                    FromTime = new TimeSpan(10, 00, 00),
                    ToTime = new TimeSpan(12, 00, 00),
                    VolunteersNeeded = 3
                },
                new SessionModel
                 {
                    FromTime = new TimeSpan(12, 00, 00),
                    ToTime = new TimeSpan(14, 00, 00),
                    VolunteersNeeded = 3
                }
            };
            return sessionModel;
        }

        private Team SetupTeam()
        {
            return new Team {Id = 1, Name = "Team Stockholm"};
        }

        private User SetUpUser()
        {
            var user = new User
            {
                Id = 1,
                Name = "Scandinavian Coder",
                Email = "coder@scandinaviancoder.com",
                Cellphone = "123456",
                Created = DateTime.Now.AddDays(-1),
                Team = new Team {Id = 1, Name = "Team Stockholm"}
            };

            return user;
        }

    }
}
