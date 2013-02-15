using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;

namespace EventBooking.Controllers
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/

        public ActionResult Checkpoint()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model)
        {
            model.ErrorMessage = "E-postadress eller lösenord är felaktigt";
            return View("Checkpoint", model);
        }
    }
}
