using Autofac;
using EventBooking.Controllers.ViewModels;
using EventBooking.Services;
using NUnit.Framework;

namespace EventBooking.Tests
{
	[SetUpFixture]
	internal class ClassSetup
	{
		[SetUp]
		public void Setup()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<MockupSecurityService>().As<ISecurityService>().InstancePerLifetimeScope();
			EventBookingMapper.SetupMappers(builder.Build());
		}
	}
}