using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity;

namespace EventBooking.Security
{
	public class DenyAuthenticatedAccessPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			return new RedirectToRouteResult(new RouteValueDictionary(new { action = "SignOff", controller = "Security" }));
		}
	}
}