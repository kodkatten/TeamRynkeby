using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class SecurityController : Controller
    {
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public SecurityController() : this(new SecurityService())
        {
            
        }

        //
        // GET: /Security/

        public ActionResult Checkpoint()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model)
        {
            var user = _securityService.GetUser(model.ElectronicMailAddress, model.Password);
            if (user == null)
            {
                model.ErrorMessage = "E-postadress eller lösenord är felaktigt";
                return View("Checkpoint", model);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignOff()
        {
            _securityService.SignOff();
            return RedirectToAction("Index", "Home");
        }
    }
}
