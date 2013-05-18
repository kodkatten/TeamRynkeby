using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
	public class ActivityRepository : IActivityRepository
	{
		private readonly IEventBookingContext _context;

		public ActivityRepository(IEventBookingContext context)
		{
			_context = context;
		}

        public IEnumerable<Activity> GetActivityByMonth(int year, int month, int teamId = 0)
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

		public virtual void Add(Activity activity)
		{
		    var user = _context.Users.Find(activity.Coordinator.Id);
		    activity.Coordinator = user;
		    activity.OrganizingTeam = _context.Teams.Find(activity.OrganizingTeam.Id);
			_context.Activities.Add(activity);
			_context.SaveChanges();
		}

        public virtual void UpdateActivity(Activity activity)
        {
            var c =_context.Activities.First(a => a.Id == activity.Id);
            c.Name = activity.Name;
            c.Summary = activity.Summary;
            c.Description = activity.Description;
            c.Type = activity.Type;
            c.Sessions = activity.Sessions;
            c.Date = activity.Date;
            c.Items = activity.Items;

            _context.SaveChanges();

        }

        public IEnumerable<Activity> GetUpcomingActivities(int skip, int take)
        {
            return _context.Activities
                                .UpcomingActivities()
                                .Where(activity => !activity.OrganizingTeam.IsDeleted)
                                .Page(skip, take);
 //                               .Include(activity => activity.OrganizingTeam);
        }

        public IEnumerable<Activity> GetUpcomingActivitiesByTeam(int teamId, int skip, int take)
        {
            return _context.Activities
                                .UpcomingActivities()
                                .Where(activity => activity.OrganizingTeam.Id == teamId && !activity.OrganizingTeam.IsDeleted)
                                .Page(skip, take);
        }

        public IEnumerable<Activity> GetUpcomingActivitiesByTeams(IEnumerable<int> teamIds, int skip, int take)
        {
            return _context.Activities
                                .UpcomingActivities()
                                .Where(activity => teamIds.Contains(activity.OrganizingTeam.Id) && !activity.OrganizingTeam.IsDeleted)
                                .Page(skip, take);
        }

	    public virtual Activity GetActivityById(int id)
	    {
	        return _context.Activities
                                .Include(x => x.OrganizingTeam)                                
                                .Include(x => x.Items)
                                .Include(x => x.Sessions.Select(session => session.Volunteers))
                                .SingleOrDefault(x => x.Id == id);
	    }
	}
}