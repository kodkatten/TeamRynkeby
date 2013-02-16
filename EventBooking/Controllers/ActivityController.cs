using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;

namespace EventBooking.Controllers
{
	public class ActivityController : Controller
	{
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create( CreateActivityModel model )
		{
			if (!ModelState.IsValid)
				return View();
			StoreActivity( Mapper.Map<Activity>( model ) );
			return RedirectToAction( "Index", "Home" );
		}

		protected virtual void StoreActivity( Activity activity )
		{
			throw new System.NotImplementedException();
		}
	}
}