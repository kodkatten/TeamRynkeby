using System.Collections.Generic;

namespace EventBooking.Data.Repositories
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetSessionsForActivity(int activityId);
        void Save(int activityId, Session session);
        void SaveVolunteers(Session session);
        Session GetSessionById(int sessionId);
    }
}