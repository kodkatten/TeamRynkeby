using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBooking.Data.Migrations;

namespace EventBooking.Data
{
	public static class DataMigrator
	{
		public static void EnableMigrations(bool isInDebugMode = false)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<EventBookingContext, Configuration>());
		}
	}
}
