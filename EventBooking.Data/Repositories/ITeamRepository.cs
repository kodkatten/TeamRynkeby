using System.Collections.Generic;

namespace EventBooking.Data.Repositories
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
    }

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
    }
}