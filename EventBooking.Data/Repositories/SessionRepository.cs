using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public class SessionRepository
	{
		public virtual IEnumerable<Session> GetSessionsForActivity(int activityId)
		{
			using (var ctx = new EventBookingContext())
				return ctx.Sessions.Where(s => s.Activity.Id == activityId).Include(s => s.Activity).Include(s => s.Activity.OrganizingTeam).ToArray();
		}

		public virtual void Save(int activityId, Session session)
		{
			using (var ctx = new EventBookingContext())
			{
				ctx.Sessions.Attach(session);
				ctx.SaveChanges();
			}
		}
	}
}