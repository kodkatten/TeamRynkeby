using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Queries;
using EventBooking.Services;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetUpcomingActivitiesQuery.Factory getUpcomingActivitiesCommandFactory;
        private readonly GetTeamActivitiesByMonthQuery.Factory getTeamActivitiesCommandFactory;
        private readonly ISecurityService _securityService;
       

        public HomeController(GetUpcomingActivitiesQuery.Factory getUpcomingActivitiesCommandFactory,
            GetTeamActivitiesByMonthQuery.Factory getTeamActivitiesCommandFactory,
            ISecurityService securityService)
        {
            this.getUpcomingActivitiesCommandFactory = getUpcomingActivitiesCommandFactory;
            this.getTeamActivitiesCommandFactory = getTeamActivitiesCommandFactory;
            this._securityService = securityService;
        }

        public ActionResult Index()
        {
            

            if (User.Identity.IsAuthenticated)
            {
                User user = _securityService.GetUser(User.Identity.Name);
                if (user.IsMemberOfATeam())
                {
                    var teamQuery = this.getTeamActivitiesCommandFactory.Invoke(DateTime.Now.Month, user.Team.Id);
                    var teamModel = new LandingPageModel(teamQuery.Execute());
                    return View(teamModel);
                }
            }

            var query = this.getUpcomingActivitiesCommandFactory.Invoke(DateTime.Now.Month);
            var model = new LandingPageModel(query.Execute());

            return View(model);
        }
    }
}
