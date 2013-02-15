namespace EventBooking.Controllers.ViewModels
{
    public class ActivityModel
    {
        public string Description { get; internal set; }
        public string DateFormatted { get; internal set; }
        public string Name { get; internal set; }
		public string OrganizingTeam { get; private set; }
    }
}