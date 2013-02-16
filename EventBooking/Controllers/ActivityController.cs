﻿using System;
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
			if ( !_securityService.IsLoggedIn )
			{
				return RedirectToAction( "Checkpoint", "Security", new { returnUrl = Url.Action( "Create" ) } );
			}
			return View();
		}

		public ActionResult Index()
		{
			return RedirectToAction( "Upcoming" );
		}

		[HttpPost]
		public ActionResult Create( CreateActivityModel model )
		{
			if ( !ModelState.IsValid )
				return View();
			var activity = Mapper.Map<Activity>( model );
			activity.OrganizingTeam = _securityService.CurrentUser.Team;
			model.Session.FromTime = activity.Date.Date.AddHours(model.Session.FromTime.Hour).AddMinutes(model.Session.FromTime.Minute);
			model.Session.ToTime = activity.Date.Date.AddHours(model.Session.ToTime.Hour).AddMinutes(model.Session.ToTime.Minute);
			activity.Sessions = new List<Session> { Mapper.Map<Session>( model.Session ) };
			StoreActivity( activity );
			return RedirectToAction( "Index", "Home" );
		}

		public static int NumberOfActivities = 10;

		public ActionResult Upcoming( int skip = 0 )
		{
			IEnumerable<Activity> query = null;
			if ( _securityService.IsLoggedIn )
			{
				var user = _securityService.CurrentUser;
				if ( user.IsMemberOfATeam() )
				{
					query = _activityRepository.GetUpcomingActivitiesByTeam( user.Team.Id, skip, NumberOfActivities );
				}
			}

			if ( query == null )
			{
				query = _activityRepository.GetUpcomingActivities( skip, NumberOfActivities );
			}

			var viewModel = new UpcomingActivitiesModel( query.ToArray() );

			return this.PartialView( viewModel );
		}

		protected virtual void StoreActivity( Activity activity )
		{
			_activityRepository.Add( activity );
		}

		public ActionResult Details(int id)
		{
			if ( !_securityService.IsLoggedIn )
			{
				return RedirectToAction( "Checkpoint", "Security", new { returnUrl = Url.Action( "Details", id ) } );
			} 
			
			var activity = _activityRepository.GetActivityById( id );

		    var viewModel = new ActivityModel(activity);

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
	}
}