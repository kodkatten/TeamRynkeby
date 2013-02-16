using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;
using EventBooking.Data.Repositories;
using System;
using System.Linq;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IActivityRepository _activities;

        public HomeController(IActivityRepository activities)
        {
            _activities = activities;
        }

        public ActionResult Index()
        {
            var query =  _activities.GetActivityByMonth(DateTime.Now.Year, DateTime.Now.Month);
            var model = new LandingPageModel(query.ToArray() /* Materialize the query */);
            return View(model);
        }
    }
}
