using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;

namespace EventBooking.Controllers
{
	public class SessionsController : Controller
	{
		private readonly SessionRepository _repository;

		public SessionsController(SessionRepository repository)
		{
			_repository = repository;
		}

		public ActionResult Index(int activityId = 0)
		{
			_repository.GetSessionsForActivity(activityId);
			return View(new ActivitySessionsModel( 
				new ActivityModel(
					new Activity { Id = 1, Name = "aoeuoe", Date = DateTime.Now}),
					new List<SessionModel>{ new SessionModel { FromTime = DateTime.Now.AddHours(-1), ToTime = DateTime.Now, VolunteersNeeded = 32}} ) );
		}

		public ActionResult Create()
		{
			throw new System.NotImplementedException();
		}
	}
}