using System.Data.Entity;

namespace EventBooking.Data
{
    public interface  IEventBookingContext
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Activity> Activities { get; set; }
        IDbSet<Team> Teams { get; set; }  
    }
}