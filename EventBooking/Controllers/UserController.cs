using System;
using System.Web.Mvc;

using EventBooking.Controllers.ViewModels;

using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class UserController : Controller
    {
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { Created = DateTime.Now });
                WebSecurity.Login(model.Email, model.Password, model.RememberMe);
                return RedirectToAction("MyProfile");
            }

            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            return View();
        }
    }
}
