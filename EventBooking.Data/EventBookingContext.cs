using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;

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
		public IDbSet<Session> Sessions { get; set; }
		public IDbSet<InterviewQuestion> InterviewQuestions { get; set; }
		public IDbSet<TrainingQuestion> TrainingQuestions { get; set; }
		public IDbSet<ActivityItemTemplate> ActivityItemTemplates { get; set; }

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
			modelBuilder.Entity<TrainingQuestion>().HasRequired(q => q.Team);
			modelBuilder.Entity<InterviewQuestion>().HasRequired(q => q.Team);

			modelBuilder
				.Entity<User>()
				.HasMany<Team>(x => x.AdminInTeams)
				.WithMany(x => x.TeamAdmins)
				.Map(x =>
				{
					x.MapLeftKey("UserId");
					x.MapRightKey("TeamId");
					x.ToTable("TeamAdmins");
				});
		}
	}
}