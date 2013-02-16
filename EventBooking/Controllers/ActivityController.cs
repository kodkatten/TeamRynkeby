using System.Web.Mvc;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class ActivityController : Controller
	{
	    private readonly ISecurityService _securityService;


	    public ActivityController(ISecurityService securityService)
	    {
	        _securityService = securityService;
	    }

	    public RedirectToRouteResult Create()
		{
			return RedirectToAction("Index", "Home");
		}

	    public ActionResult Index()
	    {
	        if (!_securityService.IsLoggedIn)
	        {
	            return RedirectToAction("Checkpoint", "Security");
	        }
	        return new ViewResult();
	    }

	    public ActionResult Details(int id)
	    {
	        return View();
	    }
	}
}