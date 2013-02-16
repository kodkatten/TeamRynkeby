using System;
using System.Linq;

namespace EventBooking.Data.Repositories
{
    internal sealed class TeamRepository : ITeamRepository
    {
        private readonly IEventBookingContext _context;

        public TeamRepository(IEventBookingContext eventBookingContext)
        {
            _context = eventBookingContext;
        }
        public IQueryable<Team> GetAllTeams()
        {
            return _context.Teams;
        }
    }
}
