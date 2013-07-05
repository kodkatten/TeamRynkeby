using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Services;
using EventBooking.Settings;

namespace EventBooking.Controllers
{
    public class SecurityController : Controller
    {
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public SecurityController()
            : this(new SecurityService(null, new EmailService(new EmailSettings())))
        {
        }


        //
        // GET: /Security/

        public ActionResult Checkpoint(string returnurl = null)
        {
            return View(new LoginModel { ReturnUrl = returnurl });
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkpoint", model);
            }
            bool signedin = _securityService.SignIn(model.ElectronicMailAddress, model.Password);
            if (!signedin)
            {
                model.ErrorMessage = "Epostadress eller lösenord är felaktigt";
                return View("Checkpoint", model);
            }
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            User user = _securityService.GetUser(model.ElectronicMailAddress);

            return RedirectToAction("Details", "Team", new { id = user.Team.Id });
        }

        public ActionResult SignOff()
        {
            _securityService.SignOff();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgottenPassword()
        {
            var model = new ForgottenPasswordViewModel();
            return View("ForgottenPassword", model);
        }

        public ActionResult ResetPassword(ForgottenPasswordViewModel model)
        {
            var user = _securityService.GetUser(model.Email);
            if (user != null)
            {
                var urlAddress = Request.Url.ToString();
                var url = urlAddress.Replace("/Security/ResetPassword", "");
                _securityService.ResetPassword(user.Email, url);

            }
            else
            {
                throw new Exception("NoUserExist");
            }

            return View();
        }


        public ActionResult ClearPassword(string token)
        {
            var model = new ForgottenPasswordViewModel { Token = token };
            return View("ClearPassword", model);
        }

        [HttpPost]
        public ActionResult SetPassword(ForgottenPasswordViewModel model)
        {
            _securityService.SetPassword(model.Token, model.Password);
            return RedirectToAction("Checkpoint", "Security");
        }
    }
}