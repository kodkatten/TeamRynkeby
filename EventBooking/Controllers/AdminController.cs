using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class AdminController : Controller
	{
		private readonly ITeamRepository _teamRepository;
		private readonly IUserRepository _userRepository;
		private readonly ISecurityService _security;

		public AdminController(ITeamRepository teamRepository, IUserRepository userRepository, ISecurityService security)
		{
			_teamRepository = teamRepository;
			_userRepository = userRepository;
			_security = security;
		}

		[HttpPost]
		public ActionResult CreateTeam(string name)
		{
			_teamRepository.CreateTeam(name);
			return Redirect("ViewTeams");
		}	
		
		public ActionResult DeleteTeam(int id)
		{
			_teamRepository.DeleteTeam(id);
			return RedirectToAction("ViewTeams");
		}
		 
		public JsonResult ToogleAdmin(int userId, int teamId)
		{
			EnsureCurrentUserIsTeamAdmin(teamId);
			
			var user = _security.GetCurrentUser();

			if (user.IsAdminForTeam(teamId))
			{
				_teamRepository.RemoveAsTeamAdmin(userId, teamId);
			}
			else
			{
				_teamRepository.AddAsTeamAdmin(userId, teamId);
			}
			
			bool isAdminForTeam = user.IsAdminForTeam(teamId);
			return Json(new { isTeamAdmin = isAdminForTeam }, JsonRequestBehavior.AllowGet);
		}

		public void ExcludeFromTeam(int userId, int teamId)
		{
			EnsureCurrentUserIsTeamAdmin(teamId);
			_userRepository.RemoveFromTeam(userId);
		}

		public ActionResult Team(int id)
		{
			EnsureCurrentUserIsPowerUserOrAdmin();

			var team = _teamRepository.TryGetTeam(id);
			
			if (team == null)
				throw new HttpException(404, "Could not find team");

			var teamModel = Mapper.Map<Team, TeamModel>(team);
			return View("ViewTeam", teamModel);
		}

		public ActionResult ViewTeams()
		{
			EnsureCurrentUserIsPowerUserOrAdmin();

			var teams = _teamRepository.GetTeams();
			AdministratorPageModel model = new AdministratorPageModel(teams);
			return View(model);
		}

		private void EnsureCurrentUserIsTeamAdmin(int teamId)
		{
			if (_security.IsLoggedIn() == false)
			{
				throw new HttpException(401, "Unauthorized");
			}

			if (_security.IsCurrentUserTeamAdminFor(teamId))
			{
				throw new HttpException(403, "Forbidden");
			}
		}

		private void EnsureCurrentUserIsPowerUserOrAdmin()
		{
			if (_security.IsLoggedIn() == false)
			{
				throw new HttpException(401, "Unauthorized");
			}

			if (_security.IsCurrentUserAdminOfAnyTeam() == false 
				&&  _security.IsCurrentUserPowerUser() == false)
			{
				throw new HttpException(403, "Forbidden");
			}
		}
	}
}
