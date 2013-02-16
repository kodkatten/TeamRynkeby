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

namespace EventBooking.Controllers
{
	public class AdminController : Controller
	{
		private readonly ITeamRepository _teamRepository;

		public AdminController(ITeamRepository teamRepository)
		{
			_teamRepository = teamRepository;
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

		public ActionResult ExcludeFromTeam(int id)
		{
			return RedirectToAction("Team");
		}

		public ActionResult Team(int id)
		{
			// TODO: check permissions
			var team = _teamRepository.TryGetTeam(id);

			if (team == null)
				throw new HttpException(404, "Could not find team");

			var teamModel = Mapper.Map<Team, TeamModel>(team);
			return View("ViewTeam", teamModel);

		}

		public ActionResult ViewTeams()
		{
			// TODO: check permissions

			var teams = _teamRepository.GetTeams();
			AdministratorPageModel model = new AdministratorPageModel(teams);
			return View(model);
		}
	}
}
