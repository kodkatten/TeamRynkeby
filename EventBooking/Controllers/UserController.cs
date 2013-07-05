using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class UserController : Controller
	{
        private readonly ISecurityService _securityService;
		private readonly IUserRepository _userRepository;
		private readonly ITeamRepository _teamRepository;

		public UserController(ISecurityService security, IUserRepository userRepository, ITeamRepository teamRepository)
		{
			_securityService = security;
			_userRepository = userRepository;
			_teamRepository = teamRepository;
		}

		public ActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SignUp(SignUpModel model)
		{
			if (ModelState.IsValid)
			{
				if (_userRepository.Exists(model.Email))
				{
					ModelState.AddModelError("Email", "Epostadressen finns redan registrerad.");
					return View();
				}

				_securityService.CreateUserAndAccount(model.Email, model.Password, created: DateTime.UtcNow);
				_securityService.SignIn(model.Email, model.Password);

				return RedirectToAction("MyProfile");
			}

			return View();
		}



		[HttpPost]
		public ActionResult MyProfile(MyProfileModel model)
		{
            if (!_securityService.IsLoggedIn())
            {
                return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("MyProfile") });
            }

			if (ModelState.IsValid)
			{
				var user = _securityService.GetCurrentUser();
				//user.Birthdate = model.Birthdate;
				user.Cellphone = model.Cellphone;
				user.City = model.City;
				user.Name = model.Name;
				user.StreetAddress = model.StreetAddress;
				if (!string.IsNullOrWhiteSpace(model.ZipCode))
				{
					// Needs validation?
					user.Zipcode = model.ZipCode.Replace(" ", string.Empty);
				}

				user.Team = model.Team;
				_userRepository.Save(user);

				return RedirectToAction("Index", "Home");
			}

			var viewModel = new MyProfileModel(model.ToUser(), _teamRepository.GetTeams());
			return View(viewModel);
		}

		public ActionResult MyProfile()
		{
            if (!_securityService.IsLoggedIn())
            {
                return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("MyProfile") });
            }

			var model = new MyProfileModel(_securityService.GetCurrentUser(), _teamRepository.GetTeams());
			return View(model);
		}
	}
}
