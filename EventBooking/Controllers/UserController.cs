using System;
using System.Web.Mvc;

using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class UserController : Controller
    {
        private readonly GetTeamsQuery.Factory getTeamsCommandFactory;

        public UserController(GetTeamsQuery.Factory getTeamsCommandFactory)
        {
            this.getTeamsCommandFactory = getTeamsCommandFactory;
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
                WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { Created = DateTime.Now });
                WebSecurity.Login(model.Email, model.Password, model.RememberMe);
                return RedirectToAction("MyProfile");
            }

            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            var query = getTeamsCommandFactory.Invoke();
            var model = new MyProfileModel(query.Execute());

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyProfile(MyProfileModel model)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            return View();
        }
    }
}
