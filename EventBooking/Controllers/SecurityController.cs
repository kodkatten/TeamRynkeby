using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Entities;
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

		public SecurityController()
			: this(new SecurityService(null))
		{
		}

		
		//
		// GET: /Security/

		public ActionResult Checkpoint(string returnurl = null)
		{
			return View(new LoginModel {ReturnUrl = returnurl});
		}

		[HttpPost]
		public ActionResult LogIn(LoginModel model)
		{
            if (!ModelState.IsValid)
            {
                return View("Checkpoint",model);
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

			return RedirectToAction("Details", "Team", new {id = user.Team.Id});
		}

		public ActionResult SignOff()
		{
			_securityService.SignOff();
			return RedirectToAction("Index", "Home");
		}
	}
}