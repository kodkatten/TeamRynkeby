using System;
using System.Web.Mvc;

using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetActivitiesByMonthQuery.Factory getActivitiesCommandFactory;

        public HomeController(GetActivitiesByMonthQuery.Factory getActivitiesCommandFactory)
        {
            this.getActivitiesCommandFactory = getActivitiesCommandFactory;
        }

        public ActionResult Index()
        {
            var query = this.getActivitiesCommandFactory.Invoke(DateTime.Now.Month);
            var model = new LandingPageModel(query.Execute());

            return View(model);
        }
    }
}
