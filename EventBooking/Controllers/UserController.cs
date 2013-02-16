using System;
using System.Web.Mvc;

using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Queries;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class UserController : Controller
    {
        private readonly GetTeamsQuery.Factory getTeamsCommandFactory;
        private readonly ISecurityService security;
        private readonly IUserRepository userRepository;

        public UserController(GetTeamsQuery.Factory getTeamsCommandFactory, ISecurityService security, IUserRepository userRepository)
        {
            this.getTeamsCommandFactory = getTeamsCommandFactory;
            this.security = security;
            this.userRepository = userRepository;
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
                security.CreateUserAndAccount(model.Email, model.Password, created: DateTime.UtcNow);
                security.SignIn(model.Email, model.Password);
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
                userRepository.Save(model.ToUser());
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
