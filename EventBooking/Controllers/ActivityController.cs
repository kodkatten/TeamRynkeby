using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class ActivityController : Controller
	{
		private readonly ISecurityService _securityService;

        private readonly IActivityRepository _activityRepository;

        public ActivityController(ISecurityService securityService, IActivityRepository activityRepository)
        {
            _securityService = securityService;
            this._activityRepository = activityRepository;
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
            IQueryable<Activity> query = null;
            if (User.Identity.IsAuthenticated)
            {
                User user = _securityService.GetUser(User.Identity.Name);
                if (user.IsMemberOfATeam())
                {
                    // TODO: Fix to accept team here!
                    query = _activityRepository.GetUpcomingActivities();
                }
            }

            if (query == null)
            {
                query = _activityRepository.GetUpcomingActivities();
            }

            var model = query.Select(data => new ActivityModel(data));

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