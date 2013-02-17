using System.Collections.Generic;
using System.Linq;
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

			Id = activityData.Id;
			Name = activityData.Name;
			OrganizingTeam = activityData.OrganizingTeam == null ? "" : activityData.OrganizingTeam.Name;
			DateFormatted = activityData.Date.ToSwedishShortDateString();
			Summary = activityData.Summary;
		}

	    public ActivityModel()
	    {
	        
	    }
		public int Id { get; private set; }

		public string DateFormatted { get; private set; }

		public string Name { get; private set; }

		public string OrganizingTeam { get; private set; }

        public string Summary { get; private set; }

        //public string Url { get; private set; }

	}

    public class DetailActivityViewModel : ActivityModel
    {
        private readonly bool isLoggedIn;

        public DetailActivityViewModel(Activity activityData, bool isLoggedIn) : base(activityData)
        {
            this.isLoggedIn = isLoggedIn;
            this.Description = activityData.Description;
            this.Sessions = activityData.Sessions != null ? activityData.Sessions.Select(AsSessionViewModel) : Enumerable.Empty<SessionViewModel>();
        }

        public string Description { get; private set; }

        public IEnumerable<SessionViewModel> Sessions { get; private set; }

        public bool ShouldShowDescription { get { return isLoggedIn;  } }

        public bool ShouldShowSessions { get { return isLoggedIn; } }

        private static SessionViewModel AsSessionViewModel(Session data)
        {
            return new SessionViewModel(data);
        }
    }

    public class SessionViewModel
    {
        public SessionViewModel(Session sessionData)
        {
            this.AvailablePlaces = 10;
            this.ToTimeFormatted = "11:23";
            this.FromTimeFormatted = "12:34";
        }

        public string FromTimeFormatted { get; set; }

        public string ToTimeFormatted { get; set; }

        public int AvailablePlaces { get; set; }

        public bool CanSignUp 
        { 
            get { return this.AvailablePlaces > 0; }
        }
    }
}