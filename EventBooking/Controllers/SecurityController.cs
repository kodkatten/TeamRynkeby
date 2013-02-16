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

        public ActionResult Checkpoint(string returnurl = null)
        {
            return View(new LoginModel { ReturnUrl = returnurl});
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model)
        {
            var signedin = _securityService.SignIn(model.ElectronicMailAddress, model.Password);
            if (!signedin)
            {
                model.ErrorMessage = "E-postadress eller lösenord är felaktigt";
                return View("Checkpoint", model);
            }
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
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
