using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetUpcomingActivitiesQuery.Factory getUpcomingActivitiesCommandFactory;

        public HomeController(GetUpcomingActivitiesQuery.Factory getUpcomingActivitiesCommandFactory)
        {
            this.getUpcomingActivitiesCommandFactory = getUpcomingActivitiesCommandFactory;
        }

        public ActionResult Index()
        {
            var query = this.getUpcomingActivitiesCommandFactory();
            var model = new LandingPageModel(query.Execute());

            return View(model);
        }
    }
}
