using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IEventBookingContext context;

        public TeamRepository(IEventBookingContext context)
        {
            this.context = context;
        }

        public IEnumerable<Team> GetTeams()
        {
            return context.Teams;
        }

	    public Team Get(int teamId)
	    {
		    return context.Teams.Find(teamId);
	    }
    }
}
