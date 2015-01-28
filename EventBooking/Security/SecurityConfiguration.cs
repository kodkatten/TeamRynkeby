using System.Web.Mvc;
using EventBooking.Controllers;
using EventBooking.Data.Entities;
using EventBooking.Services;
using FluentSecurity;

namespace EventBooking.Security
{
	public class SecurityConfiguration
	{
		public static void Configure()
		{
			var resolver = DependencyResolver.Current;
			var securityService = resolver.GetService<ISecurityService>();

			SecurityConfigurator.Configure(config =>
				{
					config.ResolveServicesUsing(resolver.GetServices);

					config.GetAuthenticationStatusFrom(securityService.IsLoggedIn);
					config.GetRolesFrom(securityService.GetRolesForCurrentUser);

					config.For<HomeController>().Ignore();
					
					config.For<SecurityController>(a => a.Checkpoint("")).Ignore();
					config.For<SecurityController>(a => a.LogIn(null)).Ignore();
					config.For<SecurityController>(a => a.SignOff()).DenyAnonymousAccess();

					config.For<UserController>().DenyAnonymousAccess();
					config.For<UserController>(a => a.SignUp()).DenyAuthenticatedAccess();

					config.For<ActivityController>().DenyAnonymousAccess();
					config.For<ActivityController>(a => a.Details(0)).Ignore();
					config.For<ActivityController>(a => a.Upcoming(0, "")).Ignore();

					config.For<TeamController>().DenyAnonymousAccess();

					config.For<SessionsController>().DenyAnonymousAccess();					
					
					config.For<AdminController>().DenyAnonymousAccess();
					config.For<AdminController>(a => a.CreateTeam("")).RequireAnyRole(UserType.Administrator.ToString());
                    config.For<AdminController>(a => a.DeleteTeam(0)).RequireAnyRole(UserType.Administrator.ToString());
                    config.For<AdminController>(a => a.ToogleAdministrator(0)).RequireAnyRole(UserType.Administrator.ToString());
                    config.For<AdminController>(a => a.ToogleTeamPowerUser(0, 0)).RequireAnyRole(UserType.Administrator.ToString());
					config.For<AdminController>(a => a.ExcludeFromTeam(0, 0)).AddPolicy(resolver.GetService<RequireAdminOrTeamPowerUser>());
					config.For<AdminController>(a => a.Team(0)).AddPolicy(resolver.GetService<RequireAdminOrPowerUser>());
					config.For<AdminController>(a => a.ViewTeams()).AddPolicy(resolver.GetService<RequireAdminOrPowerUser>());

				});
			
			GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);
		}
	}
}