using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;

namespace EventBooking.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            AdministratorPageModel model = new AdministratorPageModel(new List<Team>());
            return View(model);
        }

    }
}
