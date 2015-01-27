using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
	public class DetailActivityViewModel : ActivityModel
	{
		private readonly bool _isLoggedIn;
	    private readonly bool _isSignedUpForActivity;

		public DetailActivityViewModel(Activity activityData, User user) : base(activityData)
		{
			_isLoggedIn = user != null;
		    _isSignedUpForActivity = IsSignedUp(activityData, user);
			
            Description = activityData.Description;
			Sessions = activityData.Sessions != null ? 
				                activityData.Sessions.Select(session => AsSessionViewModel(session, user)) : 
				                Enumerable.Empty<SessionViewModel>();

		}

	    private bool IsSignedUp(Activity activityData, User user)
	    {
            if (_isLoggedIn)
	        {
	            return activityData.Sessions.Aggregate(false,
	                                                   (current, session) =>
	                                                   current || session.Volunteers.Any(v => v.Id == user.Id));
	        }
	        else
	        {
	            return false;
	        }
	    }


	    public string Description { get; private set; }

        public IEnumerable<SessionViewModel> Sessions { get; private set; }

		public bool ShouldShowDescription { get { return _isLoggedIn; } }

        public bool ShouldShowSessions { get { return _isLoggedIn; } }
        
        public bool ShouldShowItems { get { return _isLoggedIn; } }

        public bool CanBringItems { get { return _isSignedUpForActivity; } }

		private static SessionViewModel AsSessionViewModel(Session data, User user)
		{
			return new SessionViewModel(data, user);
		}
	}
}