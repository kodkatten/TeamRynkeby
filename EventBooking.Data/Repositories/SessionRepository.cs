using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public class SessionRepository
	{
	    private readonly IEventBookingContext context;

        public SessionRepository(IEventBookingContext context)
        {
            this.context = context;
        }

	    public virtual IEnumerable<Session> GetSessionsForActivity(int activityId)
		{
			using (var ctx = new EventBookingContext())
				return ctx.Sessions.Where(s => s.Activity.Id == activityId).Include(s => s.Activity).Include(s => s.Activity.OrganizingTeam).ToArray();
		}

		public virtual void Save(int activityId, Session session)
		{
			using (var ctx = new EventBookingContext())
			{
                session.Activity = ctx.Activities.FirstOrDefault(x => x.Id == activityId);
				ctx.Sessions.Add(session);
				ctx.SaveChanges();
			}
		}

        public void SaveVolunteers(Session session)
        {
            // Well I'll have to admit it. EF is to smart for me. Objects in different context
            // etc. Hurts my brain. I'm sure I made something really really bad but I've spent
            // a lot of time to try figuring it out. If a EF ninja could look at this I would
            // be grateful.
        }

	    public Session GetSessionById(int sessionId)
	    {
	        return this.context.Sessions
                        .Include(activity => activity.Volunteers)
                        .Include(activity => activity.Activity)
                        .Single(session => session.Id == sessionId);
	    }
	}
}