namespace EventBooking.Data.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<EventBookingContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(EventBooking.Data.EventBookingContext context)
		{
			//  This method will be called after migrating to the latest version.

			if (!context.Teams.Any())
			{
				// TODO: Seed data here.
			    context.Teams.Add(new Team {Name = "Team Stockholm"});
			    context.Teams.Add(new Team {Name = "Team Täby"});
			    context.Teams.Add(new Team {Name = "Team Gränna/Jkp"});
			    context.Teams.Add(new Team {Name = "Team Växjö"});
			    context.Teams.Add(new Team {Name = "Team Helsingborg"});
			    context.Teams.Add(new Team {Name = "Team Malmö"});
			    context.SaveChanges();
				
			}
            if (!context.Users.Any())
            {
                var user = new User
                    {
                        Name = "Henrik Andersson",
                        Cellphone = "123456",
                        Created = DateTime.UtcNow                        
                    };
                context.Users.Add(user);

            }
            if (!context.Activities.Any())
            {
                //var activty = new Activity
                //    {
                //        Date = DateTime.Now,
                //        Name = "testactivity",
                //        Description = "test",
                //        Summary = "Test"
                //    };
                //var ts = context.Teams.Where(t => t.Name == "Team Stockholm");

                //foreach (var team in ts)
                //{
                //    activty.OrganizingTeam = team;
                //}
                //var session = new Session {ToTime = TimeSpan.MaxValue, FromTime = TimeSpan.MinValue};
                //session.Activity = activty;

                //activty.Sessions.Add(session);
                
            }
            
		}
	}
}
