using System.Collections.Generic;
using System.Data.Entity;

namespace EventBooking.Data.Queries
{
    public class GetTeamsQuery
    {
        public delegate GetTeamsQuery Factory();

        private readonly IDbSet<Team> teams;

        public GetTeamsQuery(IEventBookingContext context)
        {
            this.teams = context.Teams;
        }

        public IEnumerable<Team> Execute()
        {
            return this.teams;
        }
    }
}