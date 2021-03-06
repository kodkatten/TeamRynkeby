﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class ActivityController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly IActivityRepository _activityRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IEmailService _emailService;
        private readonly ISessionRepository _sessionRepository;
        private const int NumberOfActivitiesPerPage = 6;

        public ActivityController(
            ISecurityService securityService,
            IActivityRepository activityRepository,
            ITeamRepository teamRepository, 
            IEmailService emailService,
            ISessionRepository sessionRepository)
        {
            _securityService = securityService;
            _activityRepository = activityRepository;
        
            _teamRepository = teamRepository;
            _emailService = emailService;
            _sessionRepository = sessionRepository;
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Upcoming");
        }

        [HttpPost]
        public ActionResult Create(CreateActivityModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            
            var activity = Mapper.Map<Activity>(model);
            activity.OrganizingTeam = _securityService.GetCurrentUser().Team;
            activity.Coordinator = _securityService.GetCurrentUser();
            StoreActivity(activity);

            _emailService.SendMail(activity.Id, EmailService.EmailType.NewActivity);

            return RedirectToAction("Details", "Activity", new { Id = activity.Id });
        }
   
        public ActionResult Upcoming(int page = 0, string teamIds = "")
        {
            page = page < 0 ? 0 : page;
            var skip = NumberOfActivitiesPerPage * page;
            IEnumerable<Activity> query = null;

            var teamIdsToFilterActivitiesOn = new List<int>();
            if (!String.IsNullOrEmpty(teamIds))
            {
                teamIdsToFilterActivitiesOn.AddRange(new List<int>(teamIds.Split(',').Select(int.Parse)));
            }

            if (_securityService.IsLoggedIn())
            {
                var user = _securityService.GetCurrentUser();
                if (user != null && user.IsMemberOfATeam())
                {
                    query = _activityRepository.GetUpcomingActivitiesByTeam(user.Team.Id, skip, NumberOfActivitiesPerPage);
                }
            }

            if (query == null)
            {
                if (teamIdsToFilterActivitiesOn.Any())
                {
                    query = _activityRepository.GetUpcomingActivitiesByTeams(teamIdsToFilterActivitiesOn, skip,
                                                                             NumberOfActivitiesPerPage);
                }
                else
                {
                    query = _activityRepository.GetUpcomingActivities(skip, NumberOfActivitiesPerPage);
                }
            }

            var viewModel = query.ToArray().Select(data => new ActivityModel(data));

            return this.PartialView(viewModel);
        }

        protected virtual void StoreActivity(Activity activity)
        {
            _activityRepository.Add(activity);
        }

        public ActionResult Details(int id)
        {
            var activity = _activityRepository.GetActivityById(id);
            var viewModel = new DetailActivityViewModel(activity, _securityService.GetCurrentUser());
            return View(viewModel);
        }

        public ActionResult WhoHasSignup(int activityId)
        {
            var activity = _activityRepository.GetActivityById(activityId);
            var viewModel = new SignedForActivityViewModel(activity, _securityService.GetCurrentUser());

            return View("WhoHasSignup", viewModel);
        }

        public ActionResult WhoHasNotSignedUp(int activityId)
        {

            var howHasNotSignedUp = HowHasNotSignedUp(activityId);

            var viewModel = new HasNotSignedUp { Users = howHasNotSignedUp, ActivityId = activityId };

            ViewBag.title = "Vem har inte anmält sig";
            return View("WhoHasNotSignedUp", viewModel);


        }

        private IEnumerable<User> HowHasNotSignedUp(int activityId)
        {
            var activity = _activityRepository.GetActivityById(activityId);

            var howHasSignedUp = activity.Sessions.SelectMany(v => v.Volunteers);
            var teamMembers = _teamRepository.GetTeamMembers(_securityService.GetCurrentUser().Team.Id).ToList();
            var howHasNotSignedUp = teamMembers.Except(howHasSignedUp);
            return howHasNotSignedUp;
        }

       
        [HttpPost]
        public ActionResult SendEmail(int id, string text)
        {
            _emailService.SendMail(id, EmailService.EmailType.InfoActivity, text);
            return new EmptyResult();
        }


        public ActionResult SendReminderMail(int activityIds)
        {
            var howHasNotSignedUp = HowHasNotSignedUp(activityIds).AsQueryable();

            _emailService.SendReminderMail(activityIds, howHasNotSignedUp, EmailService.EmailType.NewActivity);

            return RedirectToAction("WhoHasNotSignedUp", new { activityId = activityIds });
        }

        public JsonResult GetEmailPreview(int id, string text)
        {
            var preview = _emailService.GetPreview(id, EmailService.EmailType.InfoActivity, text);
            return Json(new { content = preview }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditActivity(EditActivityViewModel model)
        {
            var activity = _activityRepository.GetActivityById(model.Activity.Id);
            var sessions = _sessionRepository.GetSessionsForActivity(model.Activity.Id).OrderBy(x => x.Id).ToList();
            var sessionsFromModel = model.Sessions.OrderBy(x => x.FromTime).ToList();
            bool sameSessionTimes = sessions.SequenceEqual(sessionsFromModel, new SessionComparer());


            if (!HasSomeoneSignedUp(sessions))
            {
                activity.Name = model.Activity.Name;
                activity.Summary = model.Activity.Summary;
                activity.Description = model.Activity.Description;
                activity.Type = model.SelectedActivity;
                activity.Date = model.Activity.Date;
                activity.Sessions = model.Sessions;
            }
            else
            {
                // todo: funkar. ändrar man tiden tas allt bort
                if (!sameSessionTimes || activity.Date != model.Activity.Date)
                {
                    activity.Date = model.Activity.Date;

                    foreach (var session in sessions)
                    {
                        ICollection<User> users = session.Volunteers.ToList();
                        foreach (var user in users)
                        {
                            _sessionRepository.LeaveSession(session, user);
                        }
                    }

                    activity.Sessions = model.Sessions;
                }

               
                activity.Name = model.Activity.Name;
                activity.Summary = model.Activity.Summary;
                activity.Description = model.Activity.Description;
                activity.Type = model.SelectedActivity;
            }

            _activityRepository.UpdateActivity(activity);
            return RedirectToAction("Details", "Team");
        }


        public ViewResult Edit(int activityId)
        {
            var activity = new EditActivityViewModel
                {
                    Activity = _activityRepository.GetActivityById(activityId),
                   // ItemList = _activityItemRepository.GetTemplates(),
                    ActivityTypes = new List<string>
                        {
                            "Träning",
                            "Publikt",
                            "Teammöte",
                            "Sponsor",
                            "Preliminärt"
                        }
                };


            return View("EditActivity", activity);
        }


        private static bool HasSomeoneSignedUp(IEnumerable<Session> sessions)
        {
            var hasSomeoneSignedUp = false;

            foreach (var session in sessions)
            {
                var numberOfSignedup = session.Volunteers.Count();
                if (numberOfSignedup > 0)
                {
                    hasSomeoneSignedUp = true;
                }
            }

            return hasSomeoneSignedUp;
        }

       
    }

    public class SessionComparer : IEqualityComparer<ISession>
    {
        public bool Equals(ISession x, ISession y)
        {
            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal. 
            return x.FromTime == y.FromTime && x.ToTime == y.ToTime && x.VolunteersNeeded == y.VolunteersNeeded;
        }

        public int GetHashCode(ISession obj)
        {
            throw new NotImplementedException();
        }
    }
}
