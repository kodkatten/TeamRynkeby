using System;
using System.Data;
using System.Web.Mvc;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Controllers
{
    public class UserController : Controller
    {
        private readonly ISecurityService security;
        private readonly IUserRepository userRepository;
        private readonly ITeamRepository teamRepository;

        public UserController(ISecurityService security, IUserRepository userRepository, ITeamRepository teamRepository)
        {
            this.security = security;
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;
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
            var model = new MyProfileModel(security.CurrentUser(), teamRepository.GetTeams());
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyProfile(MyProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map(model, security.CurrentUser());

                using (var context = new EventBookingContext())
                {
                    // GIEF TEAM!!111!!
                    if (user.Team != null)
                    {
                        user.Team = context.Teams.Find(user.Team.Id);
                        if (user.Team == null)
                        {
                            string message = string.Format("Could not find team (id={0}).", user.Team.Id);
                            throw new InvalidOperationException(message);
                        }
                    }

                    context.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }

            var viewModel = new MyProfileModel(model.ToUser(), teamRepository.GetTeams());

            return View(viewModel);
        }
    }
}
