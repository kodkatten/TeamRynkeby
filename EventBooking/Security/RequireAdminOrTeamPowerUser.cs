using System.Web;
using EventBooking.Services;
using FluentSecurity;
using FluentSecurity.Policy;

namespace EventBooking.Security
{
	public class RequireAdminOrTeamPowerUser : ISecurityPolicy
	{
		private readonly ISecurityService _securityService;

		public RequireAdminOrTeamPowerUser(ISecurityService securityService)
		{
			_securityService = securityService;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			var form = HttpContext.Current.Request.Form;			
			var teamId = int.Parse(form["teamId"]);

			if (_securityService.IsCurrentUserAdministratorOrPowerUser(teamId))
			{
				return PolicyResult.CreateSuccessResult(this);				
			}

			return PolicyResult.CreateFailureResult(this, "Must be administrator or team power user");
		}
	}
}