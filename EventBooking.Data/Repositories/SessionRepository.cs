using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;

namespace EventBooking.Data.Repositories
{
	public class SessionRepository : ISessionRepository
	{
		private readonly IEventBookingContext _context;

		public SessionRepository(IEventBookingContext context)
		{
			_context = context;
		}

		public SessionRepository() { }

		public virtual IEnumerable<Session> GetSessionsForActivity(int activityId)
		{
			return _context.Sessions.Where(s => s.Activity.Id == activityId).Include(s => s.Activity).Include(s => s.Activity.OrganizingTeam).Include(s => s.Volunteers).ToArray();
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

		public bool SignUp(Session session, User user)
		{
			if (!session.IsAllowedToSignUp(user))
			{
				return false;
			}

			// Add the user to the session.
			session.Volunteers.Add(user);

			// Save the changes.
			_context.SaveChanges();

			// Return success.
			return true;
		}

		public void DeleteSession(int sessionId)
		{
			Session sessionToDelete = _context.Sessions.First(s => s.Id == sessionId);

			_context.Sessions.Remove(sessionToDelete);
			_context.SaveChanges();
		}

        public void LeaveSession(Session session, User user)
        {
            session.Volunteers.Remove(user);

            var itemsToRemove = user.Items.Where(i => i.Item.Activity.Id == session.Activity.Id).ToList();

            itemsToRemove.ForEach(i => _context.UserActivityItems.Remove(i));

            _context.SaveChanges();
        }

	    public void SaveVolunteers(Session session)
		{
			_context.SaveChanges();
		}

		public void UpdateSession(int activityId, Session session)
		{
			Session sessionToUpdate = _context.Sessions.First(s => s.Id == session.Id);

			sessionToUpdate.ToTime = session.ToTime;
			sessionToUpdate.FromTime = session.FromTime;
			sessionToUpdate.VolunteersNeeded = session.VolunteersNeeded;
			_context.SaveChanges();

		}
	}
}