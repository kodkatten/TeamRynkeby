using System;
using System.Linq;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {

        private readonly IActivityRepository _activities;
        private readonly ISecurityService _securityService;
        private readonly ITeamRepository _team;


        public HomeController(IActivityRepository activities, ISecurityService securityService, ITeamRepository team)
        {
            _activities = activities;
            _securityService = securityService;
            _team = team;
        }

        public ActionResult Index(int teamId = 0)
        {
            IQueryable<Activity> query = null;
            bool isNobody = true;

            // Got an authenticated user?
            if (_securityService.IsLoggedIn)
            {
                User user = _securityService.CurrentUser;
                if (user.IsMemberOfATeam())
                {
                    // Only get activities for the user's team.
                    query = _activities.GetActivityByMonth(DateTime.Now.Year, DateTime.Now.Month, user.Team.Id);
                    isNobody = false;
                }
            }

            if (query == null)
            {
                // Get all activities.
                query = _activities.GetActivityByMonth(DateTime.Now.Year, DateTime.Now.Month, teamId: teamId);
            }

            // Get all teams.
            ViewBag.Teams = _team.GetTeams().ToArray();

            // Create the model.
            var model = new LandingPageModel(query.ToArray());
            model.IsNobody = isNobody;
            return View(model);
        }
    }
}
