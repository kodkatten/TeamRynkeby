namespace EventBooking.Data.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<EventBooking.Data.EventBookingContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(EventBooking.Data.EventBookingContext context)
		{
			//  This method will be called after migrating to the latest version.

			if (context.Teams.Any())
			{
				// TODO: Seed data here.
			    context.Teams.Add(new Team {Name = "Team Stockholm"});
			    context.Teams.Add(new Team {Name = "Team T�by"});
			    context.Teams.Add(new Team {Name = "Team Gr�nna/Jkp"});
			    context.Teams.Add(new Team {Name = "Team V�xj�"});
			    context.Teams.Add(new Team {Name = "Team Helsingborg"});
			    context.Teams.Add(new Team {Name = "Team Malm�"});
			    context.SaveChanges();
				int nop = 0;
			}
		}
	}
}
