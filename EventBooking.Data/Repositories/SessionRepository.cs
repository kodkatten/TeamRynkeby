using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Repositories
{
    public class SessionRepository
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
    }
}