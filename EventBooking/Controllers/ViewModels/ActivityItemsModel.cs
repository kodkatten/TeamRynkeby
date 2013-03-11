namespace EventBooking.Controllers.ViewModels
{
	public class ActivityItemsModel
	{
		public int ActivityId { get; set; }
		public ActivityModel Activity { get; set; }
		public ContributedInventoryModel ContributedInventory { get; set; }

		public ActivityItemsModel()
		{
			Activity = new ActivityModel();
			ContributedInventory = new ContributedInventoryModel();
		}

		public ActivityItemsModel(ActivityModel activityModel, ContributedInventoryModel contributedInventoryModel)
		{
			Activity = activityModel;
			ActivityId = activityModel.Id;
			ContributedInventory = contributedInventoryModel;
		}
	}
}