using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class ActivityModel
	{
		public ActivityModel( Data.Activity activityData )
		{
			this.Name = activityData.Name;
			this.OrganizingTeam = activityData.OrganizingTeam.Name;
			this.DateFormatted = activityData.Date.ToSwedishShortDateString();
			this.Description = activityData.Description;
		}

		public string Description { get; private set; }

		public string DateFormatted { get; private set; }

		public string Name { get; private set; }

		public string OrganizingTeam { get; private set; }

		public string Url { get; private set; }
	}

	public class CreateActivityModel
	{
		[Display( Name = "Beskrivning" )]
		[Required( ErrorMessage = "*" )]
		public string Description { get; set; }

		[Display( Name = "Datum" )]
		[Required( ErrorMessage = "*" )]
		[FutureDate( ErrorMessage = "Du måste ange ett datum i framtiden" )]
		public DateTime Date { get; set; }

		[Display( Name = "Rubrik" )]
		[Required( ErrorMessage = "*" )]
		[EmailAddress( ErrorMessage = "Eventet måste ha en rubrik" )]
		public string Name { get; set; }

		[Display( Name = "Typ" )]
		[Required( ErrorMessage = "*" )]
		[EmailAddress( ErrorMessage = "Du måste ange typ av event" )]
		public ActivityType Type { get; set; }

		[Display( Name = "Mer information" )]
		public string Summary { get; set; }
	}

	public class FutureDate : ValidationAttribute
	{
		public override bool IsValid( object value )
		{
			if ( !( value is DateTime) )
				return false;

			var dt = (DateTime)value;

			return DateTime.Now < dt;
		}
	}
}