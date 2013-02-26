using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data;
using EventBooking.Data.Repositories;

namespace EventBooking.Data.Repositories
{
    public class SessionRepository: ISessionRepository
    {
        private readonly IEventBookingContext _context;

        public SessionRepository(IEventBookingContext context)
        {
            _context = context;
        }

        public SessionRepository() { }

        public virtual IEnumerable<Session> GetSessionsForActivity(int activityId)
        {
            return _context.Sessions.Where(s => s.Activity.Id == activityId).Include(s => s.Activity).Include(s => s.Activity.OrganizingTeam).ToArray();
        }

        public virtual void Save(int activityId, Session session)
        {
            session.Activity = _context.Activities.FirstOrDefault(x => x.Id == activityId);
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }
    
	    public Session GetSessionById(int sessionId)
	    {
	        return _context.Sessions
                        .Include(activity => activity.Volunteers)
                        .Include(activity => activity.Activity)
                        .Single(session => session.Id == sessionId);
	    }


        public void SaveVolunteers(Session session)
        {
            throw new NotImplementedException();
        }

        public void UpdateSession(int activityId,Session session)
        {
            Session sessionToUpdate = _context.Sessions.First(s => s.Id == session.Id);
            
            sessionToUpdate.ToTime = session.ToTime;
            sessionToUpdate.FromTime = session.FromTime;
            sessionToUpdate.VolunteersNeeded = session.VolunteersNeeded;
            _context.SaveChanges();

        }
    }
}