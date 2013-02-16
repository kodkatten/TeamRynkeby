using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EventBooking.Data.Repositories
{
    public sealed class ActivityRepository : IActivityRepository
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
    }
}
