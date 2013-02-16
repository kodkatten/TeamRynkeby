using System;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public class ActivityRepository : IActivityRepository
	{
		private readonly IEventBookingContext _context;

		public ActivityRepository(IEventBookingContext context)
		{
			_context = context;
		}

		public IQueryable<Activity> GetActivityByMonth(int year, int month, int teamId = 0)
		{
			IQueryable<Activity> query = _context.Activities.Include(x => x.OrganizingTeam)
			                                     .Where(x => x.Date.Year == year && x.Date.Month == month);
			if (teamId > 0)
			{
				// Filter on team as well.
				query = query.Where(x => x.OrganizingTeam.Id == teamId);
			}
			return query;
		}

		public virtual void Add(Activity activity)
		{
			_context.Activities.Add(activity);
			_context.SaveChanges();
		}
	}
}