
 using System.Collections.Generic;
﻿using System.Linq;
 using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
    	Team Get( int teamId );
	    Team CreateTeam(string name);
	    Team TryGetTeam(int teamId);
	    void DeleteTeam(int teamId);
        IQueryable<User> GetTeamMembers(int teamId);
    }
}
