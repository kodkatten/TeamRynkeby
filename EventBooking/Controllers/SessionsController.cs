using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class SessionsController : Controller
    {
        private readonly SessionRepository _repository;

	private readonly ISecurityService _securityService;

        public SessionsController(SessionRepository repository, ISecurityService securityService)
        {
            _repository = repository;
            _securityService = securityService;
        }

        public ActionResult Index(int activityId = 0)
        {
            IEnumerable<Session> sessions = _repository.GetSessionsForActivity(activityId);
            Activity activity = sessions.Select(s => s.Activity).FirstOrDefault();

            if (null == activity)
                return RedirectToAction("NotFound", new { activityId });

            return View(new ActivitySessionsModel(
                            new ActivityModel(activity),
                            sessions.Select(Mapper.Map<SessionModel>)));
        }

        [HttpPost]
        public RedirectToRouteResult Save(ActivitySessionsModel sessionModel)
        {
            int activityId = sessionModel.SelectedSession.ActivityId;
            var session = Mapper.Map<Session>(sessionModel.SelectedSession);
            _repository.Save(activityId, session);
            return RedirectToAction("Index", new { activityId });
        }

        public RedirectToRouteResult SignUp(int id)
        {
            var session = _repository.GetSessionById(id);
            var user = _securityService.CurrentUser;

            if (!session.IsAllowedToSignUp(user))
            {
                return RedirectToAction("SignUpFailed", new { id });
            }

            session.SignUp(user);
            _repository.SaveVolunteers(session);

            return RedirectToAction("SignUpSuccessful", new { id });
        }

        public ActionResult SignUpSuccessful(int id)
        {
            return View();
        }

        public ActionResult SignUpFailed(int id)
        {
            return View();
        }

        public ActionResult NotFound(int activityId)
        {
            return View(activityId);
        }
    }
}