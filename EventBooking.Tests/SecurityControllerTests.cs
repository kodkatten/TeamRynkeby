using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventBooking.Controllers;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Services;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EventBooking.Tests
{
    [TestFixture]
    public class SecurityControllerTests
    {
        private SecurityController _controller;

        private ViewResult GimmeIndexView()
        {
            _controller = _controller ?? new SecurityController();
            return _controller.Checkpoint() as ViewResult;
        }


        [Test]
        public void Index_Returns_View()
        {
            var viewResult = GimmeIndexView();
            
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(LoginModel));
        }

        [Test]
        public void When_Nobody_Signs_in_they_are_redirected_to_the_page_they_wanted_to_goto()
        {
            const string validEmail = "sami.lamti@tretton37.com";
            const string validPassword = "in yo' face!";
            ISecurityService mocksecurityService = new MockupSecurityService { AcceptedEmail = validEmail, AcceptedPassword = validPassword};
            var controller = GetSecurityController(mocksecurityService);

            const string returnurl = "www.google.com";
            var result = controller.LogIn(new LoginModel
                {
                    ElectronicMailAddress = validEmail,
                    Password = validPassword,
                    ReturnUrl = returnurl,
                }) as RedirectResult;


            Assert.IsNotNull(result);
            Assert.AreEqual(returnurl, result.Url);
        } 
        
        [Test]
        public void Given_A_return_url_when_someone_goes_to_the_checkpoint_The_login_model_will_keep_the_url()
        {
            ISecurityService mocksecurityService = new MockupSecurityService { AcceptedEmail = string.Empty, AcceptedPassword = string.Empty};
            var controller = new SecurityController(mocksecurityService);

            const string returnurl = "www.google.com";
            var result = controller.Checkpoint(returnurl) as ViewResult;


            Assert.IsNotNull(result);
            Assert.AreEqual(returnurl, ((LoginModel)result.Model).ReturnUrl);
        }

        private static SecurityController GetSecurityController(ISecurityService securityService)
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var activityController = new SecurityController(securityService)
            {
                ControllerContext = controllerContext,
                Url = new UrlHelper(controllerContext.RequestContext)
            };
            return activityController;
        }

        [Test]
        public void Given_a_nobody_When_nobody_logs_in_As_somebody_and_Exists_Then_somebody_gets_redirected_to_Team_Details_With_correct_teamId()
        {
            const string expectedEmailAddress = "dennis@jessica.com";
            const string expectedPassword = "jordnötter";
            const int expectedTeamId = 42; // goes without saying, really. REALLY.
            var mockupSecurityService = new MockupSecurityService
                {
                    AcceptedEmail = expectedEmailAddress,
                    AcceptedPassword = expectedPassword,
                    ReturnUser = new User() {Email = expectedEmailAddress, Team = new Team {Id=expectedTeamId}}
                };
            var controller = GetSecurityController(mockupSecurityService);
            var model = new LoginModel { ElectronicMailAddress = expectedEmailAddress, Password = expectedPassword };

            var result = controller.LogIn(model) as RedirectToRouteResult;

            Assert.IsNotNull(result, "We're expected a redirect");
            Assert.AreEqual("Details", result.RouteValues["action"]);
            Assert.AreEqual("Team", result.RouteValues["controller"]);
            Assert.AreEqual(expectedTeamId, result.RouteValues["id"]);
            
        }

        [Test]
        public void Given_a_nobody_When_nobody_tries_to_log_As_somebody_but_doesnt_exist_Then_return_checkpoint_view_responding_vaguely() // for sec. reasons (checkpoint!).
        {
            ISecurityService mockupSecurityService = new MockupSecurityService();
            var controller = GetSecurityController(mockupSecurityService);
            var model = new LoginModel() { ElectronicMailAddress = "a@b.c", Password = "you don't know it" };

            var result = controller.LogIn(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Checkpoint", result.ViewName);
            var resultingModel = result.Model as LoginModel;
            Assert.IsNotNull(resultingModel);
            Assert.AreEqual("E-postadress eller lösenord är felaktigt", resultingModel.ErrorMessage);
        }
    }
}
