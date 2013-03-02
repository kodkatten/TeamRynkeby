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
				int nop = 0;
			}
		}
	}
}
