using Autofac;
using EventBooking.Data.Queries;
using EventBooking.Data.Repositories;

namespace EventBooking.Data
{
    public class DataDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register core stuff.
            builder.RegisterType<EventBookingContext>().As<IEventBookingContext>().InstancePerLifetimeScope();

            // Register queries.
            builder.RegisterType<GetActivitiesByMonthQuery>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetTeamsQuery>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetUpcomingActivitiesQuery>().AsSelf().InstancePerLifetimeScope();

            // Register repositories.
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityRepository>().As<IActivityRepository>().InstancePerLifetimeScope();
        }
    }
}
