using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Queries
{
    public class GetUpcomingActivitiesQuery
    {
        public delegate GetUpcomingActivitiesQuery Factory(int skip = 0, int take = 10);

        private readonly IDbSet<Activity> activities;

        private readonly int skip;

        private readonly int take;

        public GetUpcomingActivitiesQuery(IEventBookingContext context, int skip, int take)
        {
            this.skip = skip;
            this.take = take;
            this.activities = context.Activities;
        }

        public IEnumerable<Activity> Execute()
        {
            var today = SystemTime.Now();

            return this.activities
                       .Include(activity => activity.OrganizingTeam)
                       .Where(activity => activity.Date >= today)
                       .OrderBy(activity => activity.Date)
                       .Skip(this.skip)
                       .Take(this.take);
        }
    }
}