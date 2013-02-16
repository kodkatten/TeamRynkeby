﻿using System;
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
	}
}