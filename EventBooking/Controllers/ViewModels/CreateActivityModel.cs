using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
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
		public string Name { get; set; }

		[Display( Name = "Typ" )]
		[Required( ErrorMessage = "*" )]
		public ActivityType Type { get; set; }

		[Display( Name = "Mer information" )]
		public string Summary { get; set; }

		[Display( Name = "Pass" )]
		[Required( ErrorMessage = "*" )]
		public SessionModel Session { get; set; }
	}

	public class SessionModel
	{
		public SessionModel()
		{
			VolunteersNeeded = 0;
		}

		[Display( Name = "Start" )]
		[Required( ErrorMessage = "*" )]
		public DateTime FromTime { get; set; }

		[Display( Name = "Slut" )]
		[Required( ErrorMessage = "*" )]
		[IsDateAfter("FromTime", true, ErrorMessage="Sluttiden måste vara efter starttiden")]
		public DateTime ToTime { get; set; }

		[Display( Name = "Deltagare" )]
		[Required( ErrorMessage = "*" )]
		public int VolunteersNeeded { get; set; }
	}
}