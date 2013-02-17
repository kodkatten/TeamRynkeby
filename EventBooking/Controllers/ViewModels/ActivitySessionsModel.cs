using System.Collections.Generic;

namespace EventBooking.Controllers.ViewModels
{
	public class ActivitySessionsModel
	{
		private readonly ActivityModel _activity;
		private readonly List<SessionModel> _sessions;

		public ActivitySessionsModel(ActivityModel activity, List<SessionModel> sessions)
		{
			_activity = activity;
			_sessions = sessions;
			SelectedSession = new SessionModel();
		}

		public ActivityModel Activity { get { return _activity; } }
		public SessionModel SelectedSession { get; set; }
		public List<SessionModel> Sessions { get { return _sessions; } }
	}
}