using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EventBooking.Data;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
			var activities = new[]
			{
				new Activity { Description = "Bacon ipsum dolor sit amet boudin turducken fatback pancetta kielbasa pastrami doner cow capicola short ribs drumstick tail. ", Date = DateTime.Now, Name = "Awesome aktivet uno" },
				new Activity { Description = "Ham andouille spare ribs tongue pork loin tenderloin brisket. Sausage spare ribs pork loin cow flank ground round jerky beef ribs swine rump.", Date = DateTime.Now, Name = "More awesome stuff." },
			};

            return View(activities);
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

    public class SignUpModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "Lösenorden stämmer inte med varandra.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}