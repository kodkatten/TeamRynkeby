using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;

namespace EventBooking.Controllers
{
	public class SessionsController : Controller
	{
		private readonly SessionRepository _repository;

		public SessionsController( SessionRepository repository )
		{
			_repository = repository;
		}

		public ActionResult Index( int activityId = 0 )
		{
			IEnumerable<Session> sessions = _repository.GetSessionsForActivity( activityId );
			Activity activity = sessions.Select( s => s.Activity ).FirstOrDefault();

			if ( null == activity )
				return RedirectToAction( "NotFound", new { activityId } );

			return View( new ActivitySessionsModel(
							new ActivityModel( activity ),
							sessions.Select( Mapper.Map<SessionModel> ) ) );
		}

		public RedirectToRouteResult Save( int activityId, SessionModel sessionModel )
		{
			var session = Mapper.Map<Session>( sessionModel );
			_repository.Save( activityId, session );
			return RedirectToAction( "Index", new { activityId } );
		}

		public ActionResult NotFound( int activityId )
		{
			return View( activityId );
		}
	}
}