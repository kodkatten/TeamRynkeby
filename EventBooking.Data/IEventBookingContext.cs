using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace EventBooking.Data
{
    public interface  IEventBookingContext
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Activity> Activities { get; set; }
        IDbSet<Team> Teams { get; set; }
        void SaveChanges();
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}