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
    }
}