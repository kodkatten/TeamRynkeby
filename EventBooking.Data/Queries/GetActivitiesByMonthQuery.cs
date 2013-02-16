using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Queries
{
    public class GetActivitiesByMonthQuery
    {
        public delegate GetActivitiesByMonthQuery Factory(int month);
        
        private readonly int month;
        private readonly IDbSet<Activity> activities;
       

        public GetActivitiesByMonthQuery(IEventBookingContext context, int month)
        {
            this.month = month;
            this.activities = context.Activities;
        }

        public IEnumerable<Activity> Execute()
        {
            return this.activities.Where(activity => activity.Date.Month == this.month).Include(activity => activity.OrganizingTeam);
        }
    }
}