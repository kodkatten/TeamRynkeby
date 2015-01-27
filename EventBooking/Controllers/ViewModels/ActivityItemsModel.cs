namespace EventBooking.Controllers.ViewModels
{
	public class ActivityItemsModel
	{
		public int ActivityId { get; set; }
		public ActivityModel Activity { get; set; }
		
		public ActivityItemsModel()
		{
			Activity = new ActivityModel();
			
		}

		public ActivityItemsModel(ActivityModel activityModel)
		{
			Activity = activityModel;
			ActivityId = activityModel.Id;

		}
	}
}