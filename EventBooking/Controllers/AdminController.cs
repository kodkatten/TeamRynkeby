using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;

namespace EventBooking.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult EditTeams()
        {
	        return View("EditTeams");
        }

		public ActionResult ExcludeFromTeam(int userId)
		{
			return RedirectToAction("EditTeams");
		}

		public ActionResult ViewTeam(int teamId)
        {
			// TODO: check permissions

			var team = new Team()
				{
					Activities = null,
					Id = 1,
					Name = "Team Lund",
					Volunteers = new Collection<User>()
						             {
							             new User()
								             {
									             Id = 1,
									             Name = "Fulhacke Fulhacksson"

								             }
						             }

				};

	        return View("ViewTeam", team);
        }

		public ActionResult TeamMembers()
        {
			// TODO: check permissions

            AdministratorPageModel model = new AdministratorPageModel(new List<Team>()
                {
                    new Team()
                        {
                                Activities = null,
                                Id = 1,
                                Name = "Team Lund",
                                Volunteers = new Collection<User>()
                                    {
                                        new User()
                                            {
                                                Id = 1,
                                                Name = "Fulhacke Fulhacksson"
                                                
                                            }
                                    }

                        },

                     new Team()
                        {
                                Activities = null,
                                Id = 1,
                                Name = "Team Stockholm",
                                Volunteers = new Collection<User>()
                                    {
                                        new User()
                                            {
                                                Id = 2,
                                                Name = "Bug Buggson"
                                                
                                            }
                                    }

                        }
                });



            return View(model);
        }

        [HttpGet]
        public JsonResult ListTeamMembers(int teamId)
        {
            List<User> data = new List<User>
                {
                    new User()
                        {
                            Id = 2,
                            Name = "Bug Buggson"
                            

                        },
                    new User()
                        {
                            Id = 1,
                            Name = "Fulhacke Fulhacksson"

                        }

                };
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
        }

    }
}
