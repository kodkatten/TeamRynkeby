using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
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
		
		[HttpPost]
		public JsonResult DeleteTeam(int id)
		{
			_teamRepository.DeleteTeam(id);
			return Json(new {result = string.Format("Team {0} deleted", id)});
		}
		 
		[HttpPost]
		public JsonResult ToogleTeamPowerUser(int userId, int teamId)
		{
			bool isTeamAdminNow = _security.ToogleTeamPowerUser(userId, teamId);
			return Json(new { newState = isTeamAdminNow }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult ToogleAdministrator(int userId)
		{
			bool isTeamAdminNow = _security.ToogleAdministrator(userId);
			return Json(new { newState = isTeamAdminNow }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult ExcludeFromTeam(int userId, int teamId)
		{
			_userRepository.RemoveFromTeam(userId);
			return Json(new {result = string.Format("User {0} excluded from team {1}", userId, teamId)});
		}

		public ActionResult Team(int id)
		{
			var team = _teamRepository.TryGetTeam(id);
			
			if (team == null)
				throw new HttpException(404, "Could not find team");

			var teamModel = Mapper.Map<Team, TeamModel>(team);
			return View("ViewTeam", teamModel);
		}

		public ActionResult ViewTeams()
		{
			var teams = _teamRepository.GetTeams();
			var model = new AdministratorPageModel(teams);
			return View(model);
		}

	}
}
