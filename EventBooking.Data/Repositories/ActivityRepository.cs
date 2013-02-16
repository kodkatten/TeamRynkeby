using System.Linq;
using System.Data.Entity;

namespace EventBooking.Data.Repositories
{
    internal sealed class ActivityRepository : IActivityRepository
    {
        private readonly IEventBookingContext _context;

        public ActivityRepository(IEventBookingContext context)
        {
            _context = context;
        }

        public IQueryable<Activity> GetActivityByMonth(int year, int month, int teamId = 0)
        {
            var query = _context.Activities.Include(x => x.OrganizingTeam)
                .Where(x => x.Date.Year == year && x.Date.Month == month);
            if (teamId > 0)
            {
                // Filter on team as well.
                query = query.Where(x => x.OrganizingTeam.Id == teamId);
            }
            return query;
        }

        public IQueryable<Activity> GetUpcomingActivities(int skip = 0, int take = 10)
        {
            return this._context.Activities.UpcomingActivities().Page(skip, take);
        }

        public IQueryable<Activity> GetUpcomingActivitiesByTeam(int teamId, int skip = 0, int take = 10)
        {
            return this._context.Activities
                                .Include(activity => activity.OrganizingTeam)
                                .UpcomingActivities()
                                .Where(activity => activity.OrganizingTeam.Id == teamId)
                                .Page(skip, take);
        }
    }
}
