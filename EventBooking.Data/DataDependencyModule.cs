﻿using Autofac;
using EventBooking.Data.Repositories;

namespace EventBooking.Data
{
    public class DataDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register core stuff.
            builder.RegisterType<EventBookingContext>().As<IEventBookingContext>().InstancePerLifetimeScope();

            // Register repositories.
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityRepository>().AsSelf().As<IActivityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SessionRepository>().As<ISessionRepository>().InstancePerLifetimeScope();
			builder.RegisterType<MailTemplateRepository>().As<IMailTemplateRepository>().InstancePerLifetimeScope();
        }
    }

}
