
﻿using System.Collections.Generic;
﻿using System.Linq;

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
	    void RemoveAsTeamAdmin(int userId, int teamId);
	    void AddAsTeamAdmin(int userId, int teamId);
    }
}
