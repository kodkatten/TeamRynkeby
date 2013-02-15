using System;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Queries;
using Activity = EventBooking.Data.Activity;
using System.Linq;

namespace EventBooking.Controllers
{
    public class TeamController : Controller
    {
        public ActionResult Details(int id)
        {
            var query = new GetTeamByIdQuery(id);
            var model = new TeamActivitiesModel(query.Execute());
            return View(model);
        }
    }
}