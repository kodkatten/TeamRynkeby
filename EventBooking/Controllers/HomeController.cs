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

        public HomeController(ISecurityService security, ITeamRepository team)
        {
            _securityService = securityService;
            _team = team;
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

            // Create the model.
            var model = new LandingPageModel { IsNobody = isNobody };
            return View(model);
        }
    }
}
