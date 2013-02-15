using System.Data.Entity;

namespace EventBooking.Data
{
    public class EventBookingContext : DbContext
    {
        public EventBookingContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Team> Teams { get; set; }
    }
}