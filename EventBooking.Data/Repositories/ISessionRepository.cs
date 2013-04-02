using System.Collections.Generic;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
	public interface ISessionRepository
	{
		IEnumerable<Session> GetSessionsForActivity(int activityId);
		void Save(int activityId, Session session);
		void SaveVolunteers(Session session);
		Session GetSessionById(int sessionId);
		void UpdateSession(int activityId, Session session);
		bool SignUp(Session session, User user);
		void DeleteSession(int sessionId);
	    void LeaveSession(Session session, User user);
	}
}