using System;
using System.ComponentModel.DataAnnotations;
using EventBooking.Validators;

namespace EventBooking.Controllers.ViewModels
{
	public class SessionModel
	{
		public SessionModel(int activityId)
		{
			VolunteersNeeded = 0;
			ActivityId = activityId;
		}

		public SessionModel()
		{
		}

		[Display(Name = "Start")]
		[Required(ErrorMessage = "*")]
		public TimeSpan FromTime { get; set; }

		[Display(Name = "Slut")]
		[Required(ErrorMessage = "*")]
		[IsTimeAfter("FromTime", true, ErrorMessage = "Sluttiden måste vara efter starttiden")]
		public TimeSpan ToTime { get; set; }

		[Display(Name = "Antal deltagare")]
		[Required(ErrorMessage = "*")]
		public int VolunteersNeeded { get; set; }

		public int Id { get; set; }

		public int ActivityId { get; set; }
	}
}