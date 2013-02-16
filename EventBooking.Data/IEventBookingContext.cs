using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EventBooking.Data;

namespace EventBooking.Data
{
    public interface  IEventBookingContext
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Activity> Activities { get; set; }
        IDbSet<Team> Teams { get; set; }
        IDbSet<InterviewQuestion> InterviewQuestions { get; set; }
        IDbSet<TrainingQuestion> TrainingQuestions { get; set; }
        IDbSet<PredefinedActivityItem> PredefinedActivityItems { get; set; }
        void SaveChanges();
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}