using System;
using System.Web.Mvc;

using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var query = new GetActivitiesByMonthQuery(DateTime.Now.Month);
            var model = new LandingPageModel(query.Execute());

            return View(model);
        }
    }
}
