using System;
using System.Collections.Generic;
using System.Dynamic;
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
        private readonly ISessionRepository _repository;

	    private readonly ISecurityService _securityService;

        public SessionsController(ISessionRepository repository, ISecurityService securityService)
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

        public RedirectToRouteResult SignUp(int sessionId)
        {
            var session = _repository.GetSessionById(sessionId);
            var user = _securityService.CurrentUser;
            

            if (!session.IsAllowedToSignUp(user))
            {
                return RedirectToAction("SignUpFailed", new { id = sessionId });
            }
            var userId = _securityService.CurrentUser.Id;

            session.SignUp(user);
            _repository.SaveVolunteers(session);

            return RedirectToAction("SignUpSuccessful", new { id = sessionId });
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
            throw new NotImplementedException("Delete session not implemented");
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
            //todo: Varför tappar model sitt värde härifrån och till vyn?
            _repository.UpdateSession(model.ActivityId,session);
            return RedirectToAction("Index", new{model.ActivityId});
        }
    }
}