using Autofac;
using EventBooking.Services;
using FluentSecurity;
using FluentSecurity.Policy;

namespace EventBooking
{
    public class ControllerDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SecurityService>().As<ISecurityService>();
	        builder.RegisterType<MailTemplateService>().As<IMailTemplateService>();
            builder.RegisterType<EmailService>().As<IEmailService>();
			builder.RegisterAssemblyTypes(typeof(ControllerDependencyModule).Assembly).Where(t => typeof(IPolicyViolationHandler).IsAssignableFrom(t)).InstancePerLifetimeScope().AsImplementedInterfaces();
	        builder.RegisterAssemblyTypes(typeof (ControllerDependencyModule).Assembly).Where(t => typeof (ISecurityPolicy).IsAssignableFrom(t)).InstancePerLifetimeScope().AsSelf();
        }
    }
}