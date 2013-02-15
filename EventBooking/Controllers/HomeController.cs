using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var model = new LandingPage()
                {
                    Activities = new[]
                        {
                            new Activity
                                {
                                    Description = "Bacon ipsum dolor sit amet boudin turducken fatback pancetta kielbasa pastrami doner cow capicola short ribs drumstick tail. ",
                                    DateFormatted = DateTime.Now.ToShortDateString(),
                                    Name = "Awesome aktivet uno"
                                },
                            new Activity
                                {
                                    Description = "Ham andouille spare ribs tongue pork loin tenderloin brisket. Sausage spare ribs pork loin cow flank ground round jerky beef ribs swine rump.",
                                    DateFormatted = DateTime.Now.ToShortDateString(),
                                    Name = "More awesome stuff."
                                }
                        }
                };

            return View(model);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            WebSecurity.CreateUserAndAccount(model.Email, model.Password, new {Created = DateTime.Now});
            WebSecurity.Login(model.Email, model.Password, model.RememberMe);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            throw new NotImplementedException();
        }
    }
}