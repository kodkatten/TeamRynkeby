using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class ActivityModel
	{
		public ActivityModel(Activity activityData)
		{
            if (activityData == null)
            {
                return;
            }

			Id = activityData.Id.ToString();
			Name = activityData.Name;
			OrganizingTeam = activityData.OrganizingTeam == null ? "" : activityData.OrganizingTeam.Name;
			DateFormatted = activityData.Date.ToSwedishShortDateString();
			Description = activityData.Description;
		}

		public string Id { get; private set; }

		public string Description { get; private set; }

		public string DateFormatted { get; private set; }

		public string Name { get; private set; }

		public string OrganizingTeam { get; private set; }

		public string Url { get; private set; }
	}
}