using System;
using System.Data;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
	public class UserController : Controller
	{
		private readonly ISecurityService security;
		private readonly IUserRepository userRepository;
		private readonly ITeamRepository teamRepository;

		public UserController(ISecurityService security, IUserRepository userRepository, ITeamRepository teamRepository)
		{
			this.security = security;
			this.userRepository = userRepository;
			this.teamRepository = teamRepository;
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
				if (userRepository.Exists(model.Email))
				{
					ModelState.AddModelError("Email", "E-postaddressen finns redan registrerad.");
					return View();
				}

				security.CreateUserAndAccount(model.Email, model.Password, created: DateTime.UtcNow);
				security.SignIn(model.Email, model.Password);

				return RedirectToAction("MyProfile");
			}

			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult MyProfile(MyProfileModel model)
		{
			if (ModelState.IsValid)
			{
				var user = security.CurrentUser;
				user.Birthdate = model.Birthdate;
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
				userRepository.Save(user);

				return RedirectToAction("Index", "Home");
			}

			var viewModel = new MyProfileModel(model.ToUser(), teamRepository.GetTeams());
			return View(viewModel);
		}

		public ActionResult AlreadyRegistrered(string message)
		{
			return View();
		}

		[Authorize]
		public ActionResult MyProfile()
		{
			var model = new MyProfileModel(security.CurrentUser, teamRepository.GetTeams());
			return View(model);
		}
	}
}
