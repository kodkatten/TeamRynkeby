using System;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
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

		public ActionResult Create()
		{
			if (!_securityService.IsLoggedIn)
			{
				return RedirectToAction("Checkpoint", "Security");
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

		protected virtual void StoreActivity(Activity activity)
		{
			throw new NotImplementedException();
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