using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Queries;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class ActivityController : Controller
	{
		private readonly ISecurityService _securityService;

        private readonly GetUpcomingActivitiesQuery.Factory getUpcomingEventsQueryFactory;

        public ActivityController(ISecurityService securityService, GetUpcomingActivitiesQuery.Factory getUpcomingEventsQueryFactory)
        {
            _securityService = securityService;
            this.getUpcomingEventsQueryFactory = getUpcomingEventsQueryFactory;
        }

	    public ActionResult Create()
		{
			if (!_securityService.IsLoggedIn)
			{
                return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("Create") });
			}
			return View();
		}

		[HttpPost]
		public ActionResult Create(CreateActivityModel model)
		{
			if (!ModelState.IsValid)
				return View();
			StoreActivity(Mapper.Map<Activity>(model));
			return RedirectToAction("Index", "Home");
		}
        
        public ActionResult Upcoming()
        {
            var query = this.getUpcomingEventsQueryFactory();
            var model = query.Execute().Select(data => new ActivityModel(data));

            return this.PartialView(model);
        }

		protected virtual void StoreActivity(Activity activity)
		{
			throw new NotImplementedException();
		}

	    public ActionResult Details(int id)
	    {
            if (!_securityService.IsLoggedIn)
            {
                return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("Details", id) });
            }
	        return View();
	    }



	}
}