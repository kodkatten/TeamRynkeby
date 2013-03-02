using System;
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
    public class ActivityController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly ActivityRepository _activityRepository;
        private readonly IPrefedinedItemRepository _prefedinedItems;
        private readonly ITeamRepository _teamRepository;
        private const int NumberOfActivitiesPerPage = 6;

        public ActivityController(ISecurityService securityService, ActivityRepository activityRepository
            , IPrefedinedItemRepository prefedinedItems, ITeamRepository teamRepository)
        {
            _securityService = securityService;
            _activityRepository = activityRepository;
            _prefedinedItems = prefedinedItems;
            _teamRepository = teamRepository;
        }

        public ActionResult Create()
        {
            if (!_securityService.IsLoggedIn)
            {
                return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("Create") });
            }
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
                return View();
            var activity = Mapper.Map<Activity>(model);
            activity.OrganizingTeam = _securityService.CurrentUser.Team;
            activity.Sessions = new List<Session> { Mapper.Map<Session>(model.Session) };
            activity.Coordinator = _securityService.CurrentUser;
            StoreActivity(activity);

            SendEmailToTheTeam(activity);
            return RedirectToAction("Index", "Sessions", new { activityId = activity.Id });
        }

        private void SendEmailToTheTeam(Activity activity)
        {
            var teamId = activity.OrganizingTeam.Id;
            var teamMembers = _teamRepository.GetTeamMembers(teamId);
            var toAddressToName = teamMembers.ToDictionary(teamMember => teamMember.Email,teamMember => teamMember.Name);
            
            var email = new EventBooking.email.Email();
            string text = FixTheText(activity);
            email.SendMail(toAddressToName,"noreply@team-rynkby.se",  activity.OrganizingTeam.Name,activity.Name,text);
            
        }

        private string FixTheText(Activity activity)
        {
            string message = "Sammanfattning:" + activity.Summary +"\n\r";
            message += activity.Description + "\n\r";
            message += activity.Date;
            return message;
        }

        public ActionResult Upcoming(int page = 0, string teamIds = "")
        {
            page = page < 0 ? 0 : page;
            var skip = NumberOfActivitiesPerPage * page;
            IEnumerable<Activity> query = null;

            List<int> teamIdsToFilterActivitiesOn = new List<int>();
            if (!String.IsNullOrEmpty(teamIds))
            {
                teamIdsToFilterActivitiesOn.AddRange(new List<int>(teamIds.Split(',').Select(int.Parse)));
            }

            if (_securityService.IsLoggedIn)
            {
                var user = _securityService.CurrentUser;
                if (user != null && user.IsMemberOfATeam())
                {
                    query = _activityRepository.GetUpcomingActivitiesByTeam(user.Team.Id, skip, NumberOfActivitiesPerPage);
                }
            }

            if (query == null)
            {
                if (teamIdsToFilterActivitiesOn.Any())
                    query = _activityRepository.GetUpcomingActivitiesByTeams(teamIdsToFilterActivitiesOn, skip, NumberOfActivitiesPerPage);
                else
                    query = _activityRepository.GetUpcomingActivities(skip, NumberOfActivitiesPerPage);
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

            var viewModel = new DetailActivityViewModel(activity, _securityService.CurrentUser);

            return View(viewModel);
        }

        public ActionResult SelectExistingItem()
        {
            // Create the model.
            var inventoryModel = new ContributedInventoryModel();
            inventoryModel.SuggestedItems = new List<string>();
            inventoryModel.ContributedItems = new List<ContributedInventoryItemModel>();

            // Populate the suggested prefedinedItems.
            inventoryModel.SuggestedItems.AddRange(_prefedinedItems.GetPredefinedActivityItems().Select(i => i.Name));

            // Return the view.
            return View(inventoryModel);
        }

        [HttpPost]
        public ActionResult AddContributedItem(ContributedInventoryModel model)
        {
            model.ContributedItems.Add(new ContributedInventoryItemModel
            {
                Name = model.CurrentlySelectedItem,
                Quantity = model.ItemQuantity
            });
            return View("SelectExistingItem", model);
        }

        //When entering here, should leave the activity
        //TODO: This method shall be implemented
        public ActionResult Leave(int id)
        {
            throw new NotImplementedException();
        }

        // Todo: This method shall be implemented
        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }
    }
}
