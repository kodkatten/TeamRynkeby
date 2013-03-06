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
			EnsureCurrentUserIsAdmin();

			_teamRepository.CreateTeam(name);
			return Redirect("ViewTeams");
		}	
		
		public void DeleteTeam(int id)
		{
			EnsureCurrentUserIsAdmin();
			_teamRepository.DeleteTeam(id);
		}
		 
		public JsonResult ToogleTeamPowerUser(int userId, int teamId)
		{
			EnsureCurrentUserIsAdmin();
			
			bool isTeamAdminNow = _security.ToogleTeamPowerUser(userId, teamId);
			return Json(new { newState = isTeamAdminNow }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult ToogleAdministrator(int userId)
		{
			EnsureCurrentUserIsAdmin();
			
			bool isTeamAdminNow = _security.ToogleAdministrator(userId);
			return Json(new { newState = isTeamAdminNow }, JsonRequestBehavior.AllowGet);
		}

		public void ExcludeFromTeam(int userId, int teamId)
		{
			EnsureCurrentUserIsAdminOrPowerUserInTeam(teamId);
			_userRepository.RemoveFromTeam(userId);
		}

		public ActionResult Team(int id)
		{
			EnsurePowerUserOrAdmin();

			var team = _teamRepository.TryGetTeam(id);
			
			if (team == null)
				throw new HttpException(404, "Could not find team");

			var teamModel = Mapper.Map<Team, TeamModel>(team);
			return View("ViewTeam", teamModel);
		}

		public ActionResult ViewTeams()
		{
			EnsurePowerUserOrAdmin();

			var teams = _teamRepository.GetTeams();
			AdministratorPageModel model = new AdministratorPageModel(teams);
			return View(model);
		}

		private void EnsureCurrentUserIsNotTargetUser(int userId)
		{
			if(_security.GetCurrentUser().Id == userId) 
				throw new HttpException(403, "Not allowed");
		}

		private void EnsureCurrentUserIsAdminOrPowerUserInTeam(int teamId)
		{
			if (_security.IsLoggedIn() == false)
			{
				throw new HttpException(401, "Unauthorized");
			}

			if (_security.IsCurrentUserAdministratorOrPowerUser(teamId) == false)
			{
				throw new HttpException(403, "Forbidden - Administrator or PowerUser privileges are required.");
			}	
		}

		private void EnsurePowerUserOrAdmin()
		{
			if (_security.IsLoggedIn() == false)
			{
				throw new HttpException(401, "Unauthorized");
			}

			if (_security.IsCurrentUserAdministratorOrPowerUser() == false)
			{
				throw new HttpException(403, "Forbidden - Administrator or PowerUser privileges are required.");
			}	
		}

		private void EnsureCurrentUserIsAdmin()
		{
			if (_security.IsLoggedIn() == false)
			{
				throw new HttpException(401, "Unauthorized");
			}

			if (_security.IsCurrentUserAdministrator() == false)
			{
				throw new HttpException(403, "Forbidden - Administrator privileges are required.");
			}	
		}
	}
}
