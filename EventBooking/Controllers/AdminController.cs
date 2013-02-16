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
		[HttpPost]
		public ActionResult CreateTeam(string name)
		{
			return Redirect("ViewTeams");
		}

		public ActionResult ExcludeFromTeam(int id)
		{
			return RedirectToAction("Team");
		}

		public ActionResult Team(int id)
		{
			// TODO: check permissions

			var team = Lund();
			return View("ViewTeam", team);
		}

	    public ActionResult ViewTeams()
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

	    private static Team Lund()
	    {
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
		    return team;
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
