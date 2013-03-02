using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventBooking.Controllers.ViewModels
{
	public class ContributedInventoryModel
	{
		public List<string> SuggestedItems { get; set; }
		public List<ContributedInventoryItemModel> ContributedItems { get; set; }
		public string CurrentlySelectedItem { get; set; }
		public int ItemQuantity { get; set; }
		public string Intent { get; set; }

		public ContributedInventoryModel()
		{
			this.SuggestedItems = new List<string>();
			this.ContributedItems = new List<ContributedInventoryItemModel>();
		}

	}
}