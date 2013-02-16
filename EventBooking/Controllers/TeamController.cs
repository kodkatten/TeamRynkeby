using System.Collections.ObjectModel;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class TeamController : Controller
    {
        private readonly ISecurityService _securityService;

        public TeamController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public ActionResult Details()
        {
            var user = GetUser();
            var team = GetTeam();
            var model = new TeamActivitiesModel(team);
            return View(model);
        }

        private User GetUser()
        {
            return new User();
        }

        private Team GetTeam()
        {
            return new Team { Activities = new Collection<Activity> { new Activity() } };
        }
    }
}       