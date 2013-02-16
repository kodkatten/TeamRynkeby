using Autofac;
using EventBooking.Data.Queries;
using EventBooking.Data.Repositories;

namespace EventBooking.Data
{
    public class DataDependencyModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<EventBookingContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetActivitiesByMonthQuery>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetTeamsQuery>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetTeamActivitiesByMonthQuery>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetUpcomingActivitiesQuery>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        }
    }
}
