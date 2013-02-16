using System;
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
		private readonly ActivityRepository _activityRepository;

		public ActivityController(ISecurityService securityService, ActivityRepository activityRepository)
		{
			_securityService = securityService;
			_activityRepository = activityRepository;
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
			var activity = Mapper.Map<Activity>(model);
			activity.OrganizingTeam = _securityService.CurrentUser.Team;
			StoreActivity(activity);
			return RedirectToAction("Index", "Home");
		}

		protected virtual void StoreActivity(Activity activity)
		{
			_activityRepository.Add(activity);
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