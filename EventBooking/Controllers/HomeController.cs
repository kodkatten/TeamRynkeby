using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
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