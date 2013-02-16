using System.Linq;

namespace EventBooking.Data.Repositories
{
    public interface IActivityRepository
    {
        IQueryable<Activity> GetActivityByMonth(int year, int month, int teamId = 0);

        IQueryable<Activity> GetUpcomingActivities(int skip = 0, int take = 10);

        IQueryable<Activity> GetUpcomingActivitiesByTeam(int teamId, int skip = 0, int take = 10);
    }
}
