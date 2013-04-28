using System.Collections.Generic;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> GetActivityByMonth(int year, int month, int teamId = 0);
        IEnumerable<Activity> GetUpcomingActivities(int skip = 0, int take = 10);
        IEnumerable<Activity> GetUpcomingActivitiesByTeam(int teamId, int skip = 0, int take = 10);
        IEnumerable<Activity> GetUpcomingActivitiesByTeams(IEnumerable<int> teamIds, int skip, int take);
        Activity GetActivityById(int id);
        void Add(Activity activity);
        void UpdateActivity(Activity activity);
    }
}