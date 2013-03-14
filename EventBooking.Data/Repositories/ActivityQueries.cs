using System;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
    internal static class ActivityQueries
    {
        public static IQueryable<Activity> UpcomingActivities(this IQueryable<Activity> self)
        {
            var today = SystemTime.Now().Date;

            return self.Where(activity => activity.Date >= today);
        }

        public static IQueryable<Activity> Page(this IQueryable<Activity> self, int skip, int take)
        {
            return self.OrderBy(activity => activity.Date)
                       .Skip(skip)
                       .Take(take);
        }
    }
}