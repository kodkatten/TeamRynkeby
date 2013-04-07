using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
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

       public ActionResult DetailsWithDate(DateTime currentDate)
       {
           if (!_securityService.IsLoggedIn())
           {
               return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("Details") });
           }

           var currentUser = _securityService.GetCurrentUser();

           if (null == currentUser.Team)
           {
               return RedirectToAction("MyProfile", "User");
           }

           var realTeam = _teamRepository.Get(currentUser.Team.Id);
           
           var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
           var model = new TeamActivitiesModel(realTeam, currentUser, startDate);
           return View("Details", model);
       }

	    public ActionResult Details()
	    {
	        return DetailsWithDate(DateTime.UtcNow);
	    }

        public ActionResult Previous(DateTime currentDate)
        {
            return DetailsWithDate(currentDate.AddMonths(-1));
        }

        public ActionResult Next(DateTime currentDate)
        {
            return DetailsWithDate(currentDate.AddMonths(1));
        }

        public ActionResult TeamMember()
        {
            var model = new TeamMemberViewModel();
            var currentUser = _securityService.GetCurrentUser();

            model.TeamMembers =_teamRepository.GetTeamMembers(currentUser.Team.Id);

            return View("TeamMember", model);
        }
    }
}