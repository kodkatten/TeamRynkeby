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
		private readonly IActivityItemRepository _activityItemRepository;
		private readonly ITeamRepository _teamRepository;
	    private readonly IEmailService _emailService;
	    private const int NumberOfActivitiesPerPage = 6;

		public ActivityController(ISecurityService securityService, ActivityRepository activityRepository,
			IActivityItemRepository activityItemRepository, ITeamRepository teamRepository)
		{
			_securityService = securityService;
			_activityRepository = activityRepository;
			_activityItemRepository = activityItemRepository;
			_teamRepository = teamRepository;
		   
		}

		public ActionResult Create()
		{
			if (!_securityService.IsLoggedIn())
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
			activity.OrganizingTeam = _securityService.GetCurrentUser().Team;
			activity.Sessions = new List<Session> { Mapper.Map<Session>(model.Session) };
			activity.Coordinator = _securityService.GetCurrentUser();
			StoreActivity(activity);

			return RedirectToAction("Index", "Sessions", new { activityId = activity.Id });
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
			var viewModel = new DetailActivityViewModel(activity, _securityService.GetCurrentUser());
			return View(viewModel);
		}

	

		public ActionResult Edit(int id)
		{
			throw new NotImplementedException();
		}

		public ActionResult SelectExistingItem()
		{
			// Create the model.
			var inventoryModel = new ContributedInventoryModel();
			inventoryModel.SuggestedItems = new List<string>();
			inventoryModel.ContributedItems = new List<ContributedInventoryItemModel>();
			inventoryModel.Intent = "Add"; // Default intent.

			// Populate the suggested prefedined items.
			inventoryModel.SuggestedItems.AddRange(_activityItemRepository.GetTemplates().Select(i => i.Name));

			// Return the view.
			return View(inventoryModel);
		}

		[HttpPost]
		public ActionResult UpdateContributedItem(ContributedInventoryModel model)
		{
			bool isAdding = model.Intent.Equals("Add", StringComparison.OrdinalIgnoreCase);
			bool isRemoving = model.Intent.Equals("Remove", StringComparison.OrdinalIgnoreCase);

			// No currently selected item?
			if (string.IsNullOrWhiteSpace(model.CurrentlySelectedItem))
			{
				// Return the view.
				return View("SelectExistingItem", model);
			}

			// Adding?
			if (isAdding)
			{
				// Try to find this item in the collection.
				var existing = model.ContributedItems.FirstOrDefault(
					item => item.Name.Equals(model.CurrentlySelectedItem, StringComparison.OrdinalIgnoreCase));

				if (existing != null)
				{
					// Add the quantity to the existing item.
					existing.Quantity += model.ItemQuantity;
				}
				else
				{
					// Add new item.
					model.ContributedItems.Add(new ContributedInventoryItemModel
					{
						Name = model.CurrentlySelectedItem,
						Quantity = model.ItemQuantity
					});
				}
			}
			// Removing?
			else if (isRemoving)
			{
				// Remove the currently selected item.
				string itemToRemove = model.CurrentlySelectedItem;
				model.ContributedItems.RemoveAll(x => x.Name.Equals(itemToRemove, StringComparison.OrdinalIgnoreCase));
				model.CurrentlySelectedItem = string.Empty;
			}

			// Reset the intent.
			model.Intent = "Add";

			// Clear the model state and return the model back to the view.
			this.ModelState.Clear();
			return View("SelectExistingItem", model);
		}

		public ActionResult GetSuggestedItems()
		{
			return Json(_activityItemRepository.GetTemplates().Select(i => i.Name), JsonRequestBehavior.AllowGet);
		}

	}
}
