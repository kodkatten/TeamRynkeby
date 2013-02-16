using System;
using System.Linq;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Queries;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetUpcomingActivitiesQuery.Factory getUpcomingActivitiesCommandFactory;
        private readonly IActivityRepository _activities;
        private readonly ISecurityService _security;
        public HomeController(IActivityRepository activities, ISecurityService security, GetUpcomingActivitiesQuery.Factory getUpcomingActivitiesCommandFactory)
        {
            _activities = activities;
            this.getUpcomingActivitiesCommandFactory = getUpcomingActivitiesCommandFactory;
            _security = security;
        }

        public ActionResult Index()
        {
            
            IQueryable<Activity> query = null;
            if (User.Identity.IsAuthenticated)
            {
                User user = _security.GetUser(User.Identity.Name);
                if (user.IsMemberOfATeam())
                {
                    query = _activities.GetActivityByMonth(DateTime.Now.Year, DateTime.Now.Month, user.Team.Id);
                }
            }

            if (query == null)
            {
                query = _activities.GetActivityByMonth(DateTime.Now.Year, DateTime.Now.Month);
            }

            var model = new LandingPageModel(query.ToArray() /* Materialize the query */);
            return View(model);
        }
    }
}
