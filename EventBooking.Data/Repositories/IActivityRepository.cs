using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Repositories
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> GetActivityByMonth(int year, int month, int teamId = 0);

        IEnumerable<Activity> GetUpcomingActivities(int skip = 0, int take = 10);

        IEnumerable<Activity> GetUpcomingActivitiesByTeam(int teamId, int skip = 0, int take = 10);
    }
}