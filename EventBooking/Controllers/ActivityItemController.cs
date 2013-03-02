using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Repositories;

namespace EventBooking.Controllers
{
	public class ActivityItemController : Controller
	{
		private readonly IActivityItemRepository _repository;

		public ActivityItemController(IActivityItemRepository repository)
		{
			_repository = repository;
		}

		public ActionResult SelectExistingItem()
		{
			// Create the model.
			var inventoryModel = new ContributedInventoryModel();
			inventoryModel.SuggestedItems = new List<string>();
			inventoryModel.ContributedItems = new List<ContributedInventoryItemModel>();

			// Populate the suggested activityItems.
			inventoryModel.SuggestedItems.AddRange(_repository.GetTemplates().Select(i => i.Name));

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
