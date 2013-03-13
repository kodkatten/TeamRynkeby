using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
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
}