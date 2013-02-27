using System;
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

        public DetailActivityViewModel(Activity activityData, User user) : base(activityData)
        {
            this.isLoggedIn = user != null;
            this.Description = activityData.Description;
            this.Sessions = activityData.Sessions != null ? 
                    activityData.Sessions.Select(session => AsSessionViewModel(session, user)) : 
                    Enumerable.Empty<SessionViewModel>();
        }

        public string Description { get; private set; }

        public IEnumerable<SessionViewModel> Sessions { get; private set; }

        public bool ShouldShowDescription { get { return isLoggedIn;  } }

        public bool ShouldShowSessions { get { return isLoggedIn; } }

        private static SessionViewModel AsSessionViewModel(Session data, User user)
        {
            return new SessionViewModel(data, user);
        }
    }

    public class SessionViewModel
    {
        public SessionViewModel(Session sessionData, User user)
        {
            this.AvailablePlaces = sessionData.VolunteersNeeded - sessionData.Volunteers.Count();
            this.ToTimeFormatted = sessionData.ToTime.ToString();
            this.FromTimeFormatted = sessionData.FromTime.ToString();
            this.Id = sessionData.Id;
            this.CanSignUp = sessionData.IsAllowedToSignUp(user);
        }

        public string FromTimeFormatted { get; private set; }

        public string ToTimeFormatted { get; private set; }

        public int AvailablePlaces { get; private set; }

        public bool CanSignUp { get; private set; }

        public int Id { get; private set; }
    }
}