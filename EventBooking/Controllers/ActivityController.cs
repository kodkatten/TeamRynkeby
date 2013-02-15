using System.Web.Mvc;

namespace EventBooking.Controllers
{
	public class ActivityController : Controller
	{
		public RedirectToRouteResult Create()
		{
			return RedirectToAction("Index", "Home");
		}
	}
}