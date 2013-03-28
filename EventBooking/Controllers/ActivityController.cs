using System;
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
		private const int NumberOfActivitiesPerPage = 6;

		public ActivityController(ISecurityService securityService, IActivityRepository activityRepository,
			IActivityItemRepository activityItemRepository, ITeamRepository teamRepository, IEmailService emailService)
		{
			_securityService = securityService;
			_activityRepository = activityRepository;
			_activityItemRepository = activityItemRepository;
			_teamRepository = teamRepository;
			_emailService = emailService;
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

		public JsonResult GetEmailPreview(int id, string text)
		{
			var preview = _emailService.GetPreview(id, EmailService.EmailType.InfoActivity, text);
			return Json(new {content = preview}, JsonRequestBehavior.AllowGet);
		}

	}
}
