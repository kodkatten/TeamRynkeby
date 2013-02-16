using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new LandingPageModel();
            return View(model);
        }
    }
}
