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
        private readonly ISecurityService _security;
        private readonly ITeamRepository _team;


        public HomeController(IActivityRepository activities, ISecurityService security, ITeamRepository team)
        {
            _activities = activities;
            _security = security;
            _team = team;
        }

        public ActionResult Index(int teamId = 0)
        {
            IQueryable<Activity> query = null;
            bool isNobody = true;

            // Got an authenticated user?
            if (User.Identity.IsAuthenticated)
            {
                User user = _security.GetUser(User.Identity.Name);
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
            ViewBag.Teams = _team.GetAllTeams().ToArray();

            // Create the model.
            var model = new LandingPageModel(query.ToArray());
            model.IsNobody = isNobody;
            return View(model);
        }
    }
}
