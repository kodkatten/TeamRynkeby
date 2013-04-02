using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity;

namespace EventBooking.Security
{
	public class DenyAnonymousAccessPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			var returnUrl = HttpContext.Current.Request.Path;
			return new RedirectToRouteResult(new RouteValueDictionary(new { action = "Checkpoint", controller = "Security", returnurl = returnUrl }));			
		}
	}
}