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
    /// <summary>
    /// Summary description for ActivityTest
    /// </summary>
    [TestFixture]
    public class ActivityTest
    {
        public ActivityTest()
        {}

        [SetUp]
        public void SetUp()
        {
            SecurityService = new MockupSecurityService { ReturnUser = new User { Team = new Team { Name = "The team" } } };
            EmailServices = new Moq.Mock<IEmailService>();
            _userRepository = new Mock<IUserRepository>();
            _teamRepository = new Mock<ITeamRepository>();
            _userController = new UserController(_security, _userRepository.Object, _teamRepository.Object);
            _activityRepository = new Mock<IActivityRepository>();
           

            _teamRepository.Setup(t => t.CreateTeam("Team Ninja"));
           
           

        }

        private UserController _userController;
        private Mock<IUserRepository> _userRepository;
        private Mock<ITeamRepository> _teamRepository;
        private Mock<IActivityRepository> _activityRepository;
       
        private readonly MockupSecurityService _security;

        public ActivityTest(MockupSecurityService security)
        {
            this._security = security;
            this._security.CreateUserAndAccount("ninja@tretton37.com", "awesome", DateTime.Now);
           
        }

        protected MockupSecurityService SecurityService { get; set; }
        protected Mock<IEmailService> EmailServices { get; set; }


        [Test]
        public void CreateAnActivity()
        {

            var session = new SessionModel
                {
                    FromTime = new TimeSpan(12, 00, 00),
                    ToTime = new TimeSpan(14, 00, 00),
                    VolunteersNeeded = 1
                };

            var sessions = new List<SessionModel>
                 {
                     session
                 };


            var createActivityModel = new CreateActivityModel
                {
                    Information = "Henrik testar",
                    Name = "Henrik",
                    Date = DateTime.Now.AddDays(1),
                    Sessions = sessions
                };
            var activityController = new ActivityController(SecurityService, _activityRepository.Object, _teamRepository.Object, EmailServices.Object,null);
            var id = activityController.Create(createActivityModel) as ActionResult;
           

            
           
        }
    }
}
