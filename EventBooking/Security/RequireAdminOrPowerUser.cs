using EventBooking.Services;
using FluentSecurity;
using FluentSecurity.Policy;

namespace EventBooking.Security
{
	public class RequireAdminOrPowerUser : ISecurityPolicy
	{
		private readonly ISecurityService _securityService;

		public RequireAdminOrPowerUser(ISecurityService securityService)
		{
			_securityService = securityService;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			if (_securityService.IsCurrentUserAdministratorOrPowerUser())
			{
				return PolicyResult.CreateSuccessResult(this);
			}
			return PolicyResult.CreateFailureResult(this, "Must be administrator or power user");
		}
	}
}