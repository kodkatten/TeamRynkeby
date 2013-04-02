using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Filters;
using EventBooking.Services;

namespace EventBooking.Controllers
{
	public class SessionsController : Controller
	{
		private readonly ISecurityService _securityService;
	    private readonly IActivityItemRepository _activityItemRepository;
	    private readonly IUserActivityItemRepository _userActivityItemRepository;
	    private readonly IActivityRepository _activityRepository;
		private readonly ISessionRepository _sessionRepository;


		public SessionsController(IActivityRepository activityRepository, ISessionRepository repository, ISecurityService securityService, IActivityItemRepository activityItemRepository, IUserActivityItemRepository userActivityItemRepository)
		{
			_activityRepository = activityRepository;
            _sessionRepository = repository;
			_securityService = securityService;
		    _activityItemRepository = activityItemRepository;
		    _userActivityItemRepository = userActivityItemRepository;
		}

		[ImportModelStateFromTempData]
		public ActionResult Index(int activityId = 0)
		{
            IEnumerable<Session> sessions = _sessionRepository.GetSessionsForActivity(activityId);
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
                _sessionRepository.Save(activityId, session);
			}

			return RedirectToAction("Index", new { activityId });
		}

		public RedirectToRouteResult SignUp(int sessionId)
		{
            var session = _sessionRepository.GetSessionById(sessionId);
			var user = _securityService.GetCurrentUser();

			if (session != null && user != null)
			{
				// Allowed to sign up?
				if (session.IsAllowedToSignUp(user))
				{
					// Sign the user up for the session.,
                    if (_sessionRepository.SignUp(session, user))
					{
						// Success
                        return RedirectToAction("Details", "Activity", new { id = session.Activity.Id });
					}
				}
			}

			// Sign up failed.			
            return RedirectToAction("SignUpFailed", new { sessionId = sessionId });
		}

		public ActionResult SignUpSuccessful(int activityId)
		{
            return View(activityId);
		}

        public ActionResult SignUpFailed(int sessionId)
		{
            var session = _sessionRepository.GetSessionById(sessionId);

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
            _sessionRepository.DeleteSession(sessionId);

			return RedirectToAction("Index", new { activityId });
		}

		public ActionResult Edit(int activityId, int sessionId)
		{
			// Edit sessions
            var sessionToEdit = _sessionRepository.GetSessionById(sessionId);

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

            _sessionRepository.UpdateSession(model.ActivityId, session);
			return RedirectToAction("Index", new { model.ActivityId });
		}

		public ActionResult Leave(int activityId)
        {
            var user = _securityService.GetCurrentUser();
            var sessions = _sessionRepository.GetSessionsForActivity(activityId);

            foreach (var session in sessions.Where(session => session.Activity.Id == activityId))
            {
                _sessionRepository.LeaveSession(session, user);
            }

            return RedirectToAction("Details", "Activity", new { id = activityId });
        }

        public ActionResult ContributeItems(int activityItemId, int quantity)
        {
            var user = _securityService.GetCurrentUser();
            var item = _activityItemRepository.GetItem(activityItemId);

            var userItem = new UserActivityItem
                {
                    Item = item,
                    User = user,
                    Quantity = quantity
                };

            _userActivityItemRepository.CreateOrUpdate(userItem);

            return RedirectToAction("Details", "Activity", new { id = item.Activity.Id }); 
        }
	}
}