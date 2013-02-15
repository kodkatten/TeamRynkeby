using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;
using WebMatrix.WebData;
using Activity = EventBooking.Controllers.ViewModels.Activity;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var query = new GetActivitiesByMonthQuery(DateTime.Now.Month);
            var model = new LandingPageModel(query.Execute());

            return View(model);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.Email, model.Password, new {Created = DateTime.Now});
                WebSecurity.Login(model.Email, model.Password, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            return View();
        }
    }
}
