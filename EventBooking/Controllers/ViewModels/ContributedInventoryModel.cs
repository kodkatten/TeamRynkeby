using System.Collections.Generic;

namespace EventBooking.Controllers.ViewModels
{
	public class ContributedInventoryModel
	{
		public List<string> SuggestedItems { get; set; }
		public List<ContributedInventoryItemModel> ContributedItems { get; set; }
		public string CurrentlySelectedItem { get; set; }
		public int Quantity { get; set; }
        public string Intent { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

		public ContributedInventoryModel()
		{
			this.SuggestedItems = new List<string>();
			this.ContributedItems = new List<ContributedInventoryItemModel>();
		}

		public ContributedInventoryModel(IEnumerable<string> suggestedItems, IEnumerable<ContributedInventoryItemModel> contributedItems, string intent)
		{
			SuggestedItems = new List<string>(suggestedItems);
			ContributedItems = new List<ContributedInventoryItemModel>(contributedItems);
			Intent = intent;
		}

	}
}