using System;
using System.Web.Mvc;
using EventBooking.Data;
using EventBooking.Extensions;

namespace EventBooking.Controllers
{
    public class TeamController : Controller
    {
        public ActionResult Details(int id)
        {
            return View(new Team
                {
                    Activities = new[]
                         {
                             new Activity
                                 {
                                     Name = "Fake activity",
                                     Description = "A description",
                                     Date = DateTime.Now,
                                     Summary = "A summary",
                                     Coordinator = new User {Name = "Tomten"},
                                     RequiredItems = new Item[]{},
                                     Sessions = new Session[0],
                                     Type = ActivityType.Public
                                 },
                                 new Activity
                                 {
                                     Name = "Fake activity2",
                                     Description = "A description",
                                     Date = 30.Days().FromNow(),
                                     Summary = "A summary",
                                     Coordinator = new User {Name = "Tomten"},
                                     RequiredItems = new Item[]{},
                                     Sessions = new Session[0],
                                     Type = ActivityType.Public
                                 },
                         },
                    Name = "Malmö"
                });
        }
    }
}