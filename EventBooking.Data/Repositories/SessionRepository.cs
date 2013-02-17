using System.Collections.Generic;

namespace EventBooking.Data.Repositories
{
	public class SessionRepository
	{
		public virtual IEnumerable<Session> GetSessionsForActivity(int activityId)
		{
			yield break;
		}

		public void Save(int activityId, Session session)
		{
			
		}
	}
}