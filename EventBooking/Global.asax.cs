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
            //WebSecurityInitializer.Instance.EnsureInitialize();

			var builder = new ContainerBuilder();
            builder.RegisterModule(new ControllerDependencyModule());
			builder.RegisterModule(new EasySettingsModule(ConfigurationManager.AppSettings));
			builder.RegisterModule(new DataDependencyModule());
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

#if DEBUG
			// Drop the database and recreate it.
			Database.SetInitializer(new EventBookingSeedInitializer());

			using (var context = new EventBookingContext())
			{
				context.Database.Initialize(true);
			}
#else
			Trace.WriteLine("[EF] Running migrations...");
			DataMigrator.EnableMigrations();
            Trace.WriteLine("[WebSecurity] Initializing database connection...5");
            WebSecurity.InitializeDatabaseConnection("EventBookingContext", "Users", "Id", "Email", autoCreateTables: false);
#endif
        }
	}
}