using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class TeamController : Controller
    {
        private readonly ISecurityService _securityService;
	    private readonly ITeamRepository _teamRepository;

	    public TeamController(ISecurityService securityService, ITeamRepository teamRepository)
        {
	        _securityService = securityService;
	        _teamRepository = teamRepository;
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

			var realTeam = _teamRepository.Get( team.Id );
			var model = new TeamActivitiesModel( realTeam );
			return View( model );
        }
    }
}