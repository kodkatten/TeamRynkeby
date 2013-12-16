using System;
using System.Collections;
using System.Collections.Generic;
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
    public class ActivityController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly IActivityRepository _activityRepository;
        private readonly IActivityItemRepository _activityItemRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IEmailService _emailService;
        private readonly ISessionRepository _sessionRepository;
        private const int NumberOfActivitiesPerPage = 6;

        public ActivityController(ISecurityService securityService, IActivityRepository activityRepository,
            IActivityItemRepository activityItemRepository, ITeamRepository teamRepository, IEmailService emailService, ISessionRepository sessionRepository)
        {
            _securityService = securityService;
            _activityRepository = activityRepository;
            _activityItemRepository = activityItemRepository;
            _teamRepository = teamRepository;
            _emailService = emailService;
            _sessionRepository = sessionRepository;
        }

        public ActionResult Create()
        {
            return View(_activityItemRepository.GetTemplates());
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
                return View(_activityItemRepository.GetTemplates());
            }

            Mapper.CreateMap<ContributedInventoryModel, ActivityItem>()
                  .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                  .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                  .ForMember(dest => dest.Quantity, options => options.MapFrom(source => source.Quantity));


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

        [ImportModelStateFromTempData]
        public ActionResult SelectExistingItem(int activityId)
        {
            Activity activity = _activityRepository.GetActivityById(activityId);
            IEnumerable<ActivityItem> items = _activityItemRepository.GetItemsForActivity(activityId);

            var inventoryModel = new ContributedInventoryModel
                (new List<string>(_activityItemRepository.GetTemplates().Select(i => i.Name)),
                items.Select(Mapper.Map<ContributedInventoryItemModel>),
                "Add");

            var viewModel = new ActivityItemsModel(new ActivityModel(activity), inventoryModel);
            return View(viewModel);
        }

        [HttpPost, ExportModelStateToTempData]
        public ActionResult UpdateContributedItem(ActivityItemsModel model)
        {
            bool isAdding = model.ContributedInventory.Intent.Equals("Add", StringComparison.OrdinalIgnoreCase);
            bool isRemoving = model.ContributedInventory.Intent.Equals("Remove", StringComparison.OrdinalIgnoreCase);

            // No currently selected item?
            if (string.IsNullOrWhiteSpace(model.ContributedInventory.CurrentlySelectedItem))
            {
                ModelState.AddModelError("ContributedInventory.CurrentlySelectedItem", "Måste ange materiel-namn!");
                return RedirectToAction("SelectExistingItem", new { activityId = model.ActivityId });
            }

            // Adding?
            if (isAdding)
            {
                _activityItemRepository.AddOrUpdateItem(model.ActivityId,
                                                                       model.ContributedInventory.CurrentlySelectedItem,
                                                                       model.ContributedInventory.Quantity);
            }
            // Removing?
            else if (isRemoving)
            {
                // Remove the currently selected item.
                string itemToRemove = model.ContributedInventory.CurrentlySelectedItem;
                _activityItemRepository.DeleteItemByActivityIdAndItemName(model.ActivityId, itemToRemove);
            }

            // Clear the model state and return the model back to the view.
            this.ModelState.Clear();
            return RedirectToAction("SelectExistingItem", new { activityId = model.ActivityId });
        }

        public JsonResult GetSuggestedItems()
        {
            return Json(_activityItemRepository.GetTemplates().Select(i => i.Name), JsonRequestBehavior.AllowGet);
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
            var activityItems = _activityItemRepository.GetItemsForActivity(model.Activity.Id);
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
                activity.Items = model.Items;
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

                if (!HasSomeoneSignedUp(sessions))
                {
                    activity.Items = model.Items;
                }

                activity.Name = model.Activity.Name;
                activity.Summary = model.Activity.Summary;
                activity.Description = model.Activity.Description;
                activity.Type = model.SelectedActivity;
            }

            _activityRepository.UpdateActivity(activity);
            return RedirectToAction("Details", "Team");
        }


        public ActionResult Edit(int activityId)
        {
            var activity = new EditActivityViewModel
                {
                    Activity = _activityRepository.GetActivityById(activityId),
                    ItemList = _activityItemRepository.GetTemplates(),
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

        private bool HasItemQuantityInCreased(ActivityItem item, IEnumerable<ActivityItem> modelItems)
        {

            foreach (var modelItem in modelItems)
            {
                if (modelItem.Name == item.Name)
                {
                    if (modelItem.Quantity < item.Quantity)
                    {
                        return true;
                    }
                }

            }
            return false;
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
