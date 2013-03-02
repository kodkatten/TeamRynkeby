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
		public IDbSet<PredefinedActivityItem> PredefinedActivityItems { get; set; }

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
		}

		public bool IsAttached<T>(T entity)
		{
			var context = ((IObjectContextAdapter)this).ObjectContext;
			var entitySet = this.GetEntitySet(context, typeof(T));
			EntityKey key = context.CreateEntityKey(entitySet.Name, entity);
			ObjectStateEntry entry = null;
			if (context.ObjectStateManager.TryGetObjectStateEntry(key, out entry))
			{
				return entry.State != EntityState.Detached;
			}
			return false;
		}

		private EntitySetBase GetEntitySet(ObjectContext context, Type entityType)
		{
			var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
			return container.BaseEntitySets.FirstOrDefault(item => item.ElementType.Name.Equals(entityType.Name));
		}

	}
}