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

		public ActivityController(ISecurityService securityService, ActivityRepository activityRepository, IPrefedinedItemRepository prefedinedItems)
		{
			_securityService = securityService;
			_activityRepository = activityRepository;
			_prefedinedItems = prefedinedItems;
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
		    activity.Coordinator = _securityService.CurrentUser;
			StoreActivity(activity);
			return RedirectToAction("Index", "Home");
		}

		public static int NumberOfActivities = 10;

		public ActionResult Upcoming(int skip = 0)
		{
			IEnumerable<Activity> query = null;
			if (_securityService.IsLoggedIn)
			{
				var user = _securityService.CurrentUser;
				if (user.IsMemberOfATeam())
				{
					query = _activityRepository.GetUpcomingActivitiesByTeam(user.Team.Id, skip, NumberOfActivities);
				}
			}

			if (query == null)
			{
				query = _activityRepository.GetUpcomingActivities(skip, NumberOfActivities);
			}

			var viewModel = new UpcomingActivitiesModel(query.ToArray());

			return this.PartialView(viewModel);
		}

		protected virtual void StoreActivity(Activity activity)
		{
			_activityRepository.Add(activity);
		}

		public ActionResult Details(int id)
		{
			if (!_securityService.IsLoggedIn)
			{
				return RedirectToAction("Checkpoint", "Security", new { returnUrl = Url.Action("Details", id) });
			}
			return View();
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
	}
}