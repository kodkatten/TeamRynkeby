using System.Data.Entity;

namespace EventBooking.Data
{
    public class EventBookingContext : DbContext, IEventBookingContext
    {
        public EventBookingContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<Team> Teams { get; set; }
    }
}