using System.Data.Entity;
using EventBooking.Models;

namespace EventBooking.DatabaseContexts
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> UserProfiles { get; set; }
    }
}