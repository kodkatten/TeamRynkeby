using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Queries
{
    public class GetTeamActivitiesByMonthQuery
    {
        public delegate GetTeamActivitiesByMonthQuery Factory(int month, int teamId);
        
        private readonly int month;
        private int teamId;

        private readonly IDbSet<Activity> activities;
        

        public GetTeamActivitiesByMonthQuery(EventBookingContext context, int month, int teamId)
        {
            this.month = month;
            this.teamId = teamId;
            this.activities = context.Activities;
        }

        public IEnumerable<Activity> Execute()
        {
            return this.activities
                .Where(activity => activity.Date.Month == this.month
                    && activity.Coordinator.Team.Id == teamId)
                    .Include(activity => activity.OrganizingTeam);

        }
    }
}