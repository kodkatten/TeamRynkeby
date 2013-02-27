using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBooking.Controllers.ViewModels;
using EventBooking.Validators;
using NUnit.Framework;

namespace EventBooking.Tests
{
    [TestFixture]
    public class ValidationTests
    {

        private IList<SessionModel> sessions;

        [SetUp]
        public void SetUp()
        {
            sessions = new List<SessionModel>()
                {
                                 
                    new SessionModel()
            { 
                FromTime = new TimeSpan(10, 0,0),
                ToTime = new TimeSpan(12,0,0)
            },                         
            new SessionModel()
            { 
                FromTime = new TimeSpan( 8, 0,0),
                ToTime = new TimeSpan( 10,0,0)
            }
                };
        }


        [Test]
        public void ValidateThatYouCannotCreateASessionWithinAnExistingSessionsTimeFrame()
        {
            //Arrange
            SessionModel newSession = new SessionModel()
            {
                FromTime = new TimeSpan(10, 0, 0),
                ToTime = new TimeSpan(12, 0, 0)
            };



            CustomValidator customValidator = new CustomValidator();

            //Act
            bool result = customValidator.IsSessionIntrudingOnOtherSessionsTimeframe(newSession, sessions);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateThatYouCanCreateASessionThatIsNotIntrudingOnAnotherSessionsTimeframe()
        {
            //Arrange
            SessionModel newSession = new SessionModel()
            {
                FromTime = new TimeSpan(12, 0, 0),
                ToTime = new TimeSpan(13, 0, 0)
            };



            CustomValidator customValidator = new CustomValidator();

            //Act
            bool result = customValidator.IsSessionIntrudingOnOtherSessionsTimeframe(newSession, sessions);

            //Assert
            Assert.IsFalse(result);
        }


        [Test]
        public void ValidateThatYouCanCreateASessionThatIsNotIntrudingOnAnotherSessionsTimeframe2()
        {
            //Arrange
            SessionModel newSession = new SessionModel()
            {
                FromTime = new TimeSpan(7, 0, 0),
                ToTime = new TimeSpan(8, 0, 0)
            };



            CustomValidator customValidator = new CustomValidator();

            //Act
            bool result = customValidator.IsSessionIntrudingOnOtherSessionsTimeframe(newSession, sessions);

            //Assert
            Assert.IsFalse(result);
        }


        [Test]
        public void ValidateThatYouCantCreateASessionThatHasAStartTimeThatIntrudesInAExistingSession()
        {
            //Arrange
            SessionModel newSession = new SessionModel()
            {
                FromTime = new TimeSpan(11, 0, 0),
                ToTime = new TimeSpan(13, 0, 0)
            };


            CustomValidator customValidator = new CustomValidator();

            //Act
            bool result = customValidator.IsSessionIntrudingOnOtherSessionsTimeframe(newSession, sessions);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
