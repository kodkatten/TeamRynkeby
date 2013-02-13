using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Filters;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    [InitializeSimpleMembership]
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
            WebSecurity.CreateUserAndAccount(model.Email, model.Password);
            WebSecurity.Login(model.Email, model.Password);
            return RedirectToAction("Index", "Home");
        }
    }

    public class SignUpModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Lösenorden stämmer inte med varandra.")]
        public string ConfirmPassword { get; set; }
    }
}
