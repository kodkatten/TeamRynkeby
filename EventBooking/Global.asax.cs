using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using CoderMike.Autofac.EasySettings;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Filters;
using EventBooking.Security;

namespace EventBooking
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			var builder = new ContainerBuilder();			
			builder.RegisterModule(new EasySettingsModule(ConfigurationManager.AppSettings));
			builder.RegisterModule(new DataDependencyModule());
			builder.RegisterModule(new ControllerDependencyModule());
			builder.RegisterControllers(typeof(MvcApplication).Assembly);

			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			AreaRegistration.RegisterAllAreas();
			HtmlHelper.ClientValidationEnabled = true;
			HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			EventBookingMapper.SetupMappers(container);

			SecurityConfiguration.Configure();

			Database.SetInitializer(new EventBookingSeedInitializer());
			using (var context = new EventBookingContext())
			{
				context.Database.Initialize(true);
			}
		}
	}
}