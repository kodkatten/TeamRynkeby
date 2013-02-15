namespace EventBooking.Controllers.ViewModels
{
    public class ActivityModel
    {
        public ActivityModel(Data.Activity activityData)
        {
            this.Name = activityData.Name;
            this.OrganizingTeam = activityData.OrganizingTeam.Name;
            this.DateFormatted = activityData.Date.ToShortDateString();
            this.Description = activityData.Description;
        }

        public string Description { get; private set; }

        public string DateFormatted { get; private set; }

        public string Name { get; private set; }

		public string OrganizingTeam { get; private set; }

		public string Url { get; private set; }
	}
}