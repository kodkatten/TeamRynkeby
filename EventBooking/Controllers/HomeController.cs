using System.Linq;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly ITeamRepository _team;
        private readonly ActivityRepository _activityRepository;

        public HomeController(ISecurityService security, ITeamRepository team, ActivityRepository activityRepository)
        {
            _securityService = security;
            _team = team;
            _activityRepository = activityRepository;
        }

        public ActionResult Index(int teamId = 0)
        {
            bool isNobody = true;

            // Got an authenticated user?
            if (_securityService.IsLoggedIn)
            {
                var user =  _securityService.CurrentUser;
                if (user.IsMemberOfATeam())
                {
                    isNobody = false;
                }
            }

            // Get all teams.
            ViewBag.Teams = _team.GetTeams().ToArray();

            var nextTwoActivities = _activityRepository.GetUpcomingActivities(0, 2);

            // Create the model.
            var model = new LandingPageModel(nextTwoActivities) { IsNobody = isNobody };
            return View(model);
        }
    }
}
