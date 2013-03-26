using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISecurityService _securityService;
		private readonly ITeamRepository _teamRepository;
		private readonly ActivityRepository _activityRepository;

		public HomeController(ISecurityService security, ITeamRepository teamRepository, ActivityRepository activityRepository)
		{
			_securityService = security;
			_teamRepository = teamRepository;
			_activityRepository = activityRepository;
		}

		public ActionResult Index(int teamId = 0)
		{
			bool isNobody = true;

            if (_securityService.IsLoggedIn())
			{
				var user = _securityService.GetCurrentUser();
				if (user != null && user.IsMemberOfATeam())
				{
					isNobody = false;
				}
			}

			var nextTwoActivities = _activityRepository.GetUpcomingActivities(0, 2);

			var model = new LandingPageModel(nextTwoActivities, _teamRepository.GetTeams()) { IsNobody = isNobody };
			return View(model);
		}
	}
}
