using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
	public class DetailActivityViewModel : ActivityModel
	{
		private readonly bool isLoggedIn;
	    private readonly bool isSignedUpForActivity;

		public DetailActivityViewModel(Activity activityData, User user) : base(activityData)
		{
			this.isLoggedIn = user != null;
		    this.isSignedUpForActivity = IsSignedUp(activityData, user);
			this.Description = activityData.Description;
			this.Sessions = activityData.Sessions != null ? 
				                activityData.Sessions.Select(session => AsSessionViewModel(session, user)) : 
				                Enumerable.Empty<SessionViewModel>();

            this.Items = activityData.Items != null ?
                    activityData.Items.Select(item => AsItemViewModel(item, activityData, user)) :
                    Enumerable.Empty<ContributedInventoryItemModel>();

		}

	    private bool IsSignedUp(Activity activityData, User user)
	    {
	        return activityData.Sessions.Aggregate(false, (current, session) => current || session.Volunteers.Any(v => v.Id == user.Id));
	    }


	    public string Description { get; private set; }

        public IEnumerable<SessionViewModel> Sessions { get; private set; }

        public IEnumerable<ContributedInventoryItemModel> Items { get; private set; }

		public bool ShouldShowDescription { get { return isLoggedIn;  } }

        public bool ShouldShowSessions { get { return isLoggedIn; } }
        
        public bool ShouldShowItems { get { return isLoggedIn; } }

        public bool CanBringItems { get { return isSignedUpForActivity; } }

		private static SessionViewModel AsSessionViewModel(Session data, User user)
		{
			return new SessionViewModel(data, user);
		}

        private static ContributedInventoryItemModel AsItemViewModel(ActivityItem data, Activity activityData, User user)
        {
            return new ContributedInventoryItemModel(data, activityData, user);
        }
	}
}