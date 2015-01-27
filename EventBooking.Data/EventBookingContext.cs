using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EventBooking.Data.Entities;

namespace EventBooking.Data
{
	public class EventBookingContext : DbContext, IEventBookingContext
	{
		public EventBookingContext()
            : base("EventBookingContext")
		{
		}

		public IDbSet<User> Users { get; set; }
		public IDbSet<Activity> Activities { get; set; }
		public IDbSet<Team> Teams { get; set; }
		public IDbSet<Session> Sessions { get; set; }
		public IDbSet<MailTemplate> MailTemplates { get; set; }
	   

	    void IEventBookingContext.SaveChanges()
		{
			this.SaveChanges();
		}

		DbEntityEntry IEventBookingContext.Entry<TEntity>(TEntity entity)
		{
			return this.Entry(entity);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			
		}
	}
}