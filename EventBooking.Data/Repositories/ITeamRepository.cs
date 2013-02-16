
﻿using System.Collections.Generic;
﻿using System.Linq;

namespace EventBooking.Data.Repositories
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
    }
}
