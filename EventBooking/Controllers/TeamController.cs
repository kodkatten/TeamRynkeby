using System.Data.Entity;
using System.Linq;
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
            if (!_securityService.IsLoggedIn)
            {
                return RedirectToAction("Checkpoint", "Security", new {returnUrl = Url.Action("Details")});
            }

            var team = _securityService.CurrentUser.Team;

            if (null == team)
            {
                return RedirectToAction("MyProfile", "User");
            }

            using (var context = new EventBookingContext())
            {
                var realTeam = context.Teams.Where(t => t.Id == team.Id).Include(t => t.Activities).First();
                var model = new TeamActivitiesModel(realTeam);
                return View(model);
            }
        }
    }
}