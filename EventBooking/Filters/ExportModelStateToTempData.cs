using System.Web.Mvc;

namespace EventBooking.Filters
{
	public class ExportModelStateToTempData : ModelStateTempDataTransfer
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			//Only export when ModelState is not valid
			if (!filterContext.Controller.ViewData.ModelState.IsValid)
			{
				//Export if we are redirecting
				if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
				{
					filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
				}
			}

			base.OnActionExecuted(filterContext);
		}
	}
}