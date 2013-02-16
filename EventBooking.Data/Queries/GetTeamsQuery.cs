using System.Collections.Generic;
using System.Data.Entity;

namespace EventBooking.Data.Queries
{
    public class GetTeamsQuery
    {
        public delegate GetTeamsQuery Factory();

        private readonly DbSet<Team> teams;

        public GetTeamsQuery(EventBookingContext context)
        {
            this.teams = context.Teams;
        }

        public IEnumerable<Team> Execute()
        {
            return this.teams;
        }
    }
}