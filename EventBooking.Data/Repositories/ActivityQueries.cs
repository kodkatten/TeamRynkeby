using System.Linq;

namespace EventBooking.Data.Repositories
{
    internal static class ActivityQueries
    {
        public static IQueryable<Activity> UpcomingActivities(this IQueryable<Activity> self)
        {
            var today = SystemTime.Now();

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