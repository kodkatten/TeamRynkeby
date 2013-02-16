using Autofac;
using EventBooking.Services;

namespace EventBooking
{
    public class ControllerDependencyModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SecurityService>().As<ISecurityService>();
        }
    }
}