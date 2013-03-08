using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Extensions;
using EventBooking.Filters;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class SessionsController : Controller
	{
		private readonly IActivityRepository _activityRepository;
		private readonly ISessionRepository _repository;

		private readonly ISecurityService _securityService;

		public SessionsController(IActivityRepository activityRepository, ISessionRepository repository, ISecurityService securityService)
		{
			_activityRepository = activityRepository;
			_repository = repository;
			_securityService = securityService;
		}

		[ImportModelStateFromTempData]
		public ActionResult Index(int activityId = 0)
		{
			IEnumerable<Session> sessions = _repository.GetSessionsForActivity(activityId);
			Activity activity = _activityRepository.GetActivityById(activityId);

			if (null == activity)
				return RedirectToAction("NotFound", new { activityId });

			return View(new ActivitySessionsModel(
							new ActivityModel(activity),
							sessions.Select(Mapper.Map<SessionModel>)));
		}

		[HttpPost, ExportModelStateToTempData]
		public ActionResult Save(ActivitySessionsModel sessionModel)
		{
			int activityId = sessionModel.SelectedSession.ActivityId;

			if (ModelState.IsValid)
			{
				var session = Mapper.Map<Session>(sessionModel.SelectedSession);
				_repository.Save(activityId, session);
			}

			return RedirectToAction("Index", new { activityId });
		}

		public RedirectToRouteResult SignUp(int sessionId)
		{
			var session = _repository.GetSessionById(sessionId);
			var user = _securityService.GetCurrentUser();

			if (session != null && user != null)
			{
				// Allowed to sign up?
				if (session.IsAllowedToSignUp(user))
				{
					// Sign the user up for the session.,
					if (_repository.SignUp(session, user))
					{
						// Success
						return RedirectToAction("SignUpSuccessful", new { id = sessionId });
					}
				}
			}

			// Sign up failed.			
			return RedirectToAction("SignUpFailed", new { id = sessionId });
		}

		public ActionResult SignUpSuccessful(int id)
		{
			return View();
		}

		public ActionResult SignUpFailed(int id)
		{
			var session = _repository.GetSessionById(id);

			dynamic viewModel = new ExpandoObject();
			viewModel.ActivityId = session.Activity.Id;

			return View(viewModel);
		}

		public ActionResult NotFound(int activityId)
		{
			return View(activityId);
		}

		public ActionResult Delete(int activityId, int sessionId)
		{
			_repository.DeleteSession(sessionId);

			return RedirectToAction("Index", new { activityId });
		}

		public ActionResult Edit(int activityId, int sessionId)
		{
			// Edit sessions
			var sessionToEdit = _repository.GetSessionById(sessionId);

			var editSessionModel = new EditSessionModel
				{
					ActivityId = activityId,
					SessionId = sessionToEdit.Id,
					ToTime = sessionToEdit.ToTime,
					FromTime = sessionToEdit.FromTime,
					VolunteersNeeded = sessionToEdit.VolunteersNeeded
				};

			return View("EditSession", editSessionModel);

		}
		public ActionResult Update(EditSessionModel model)
		{

			var session = new Session
				{
					Id = model.SessionId,
					FromTime = model.FromTime,
					ToTime = model.ToTime,
					VolunteersNeeded = model.VolunteersNeeded
				};

			_repository.UpdateSession(model.ActivityId, session);
			return RedirectToAction("Index", new { model.ActivityId });
		}
	}
}