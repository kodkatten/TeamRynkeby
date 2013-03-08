using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventBooking.Data;
using EventBooking.Validators;

namespace EventBooking.Controllers.ViewModels
{
	public class CreateActivityModel
	{
		[Display(Name = "Beskrivning")]
		[Required(ErrorMessage = "*")]
		public string Description { get; set; }

		[Display(Name = "Datum")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required(ErrorMessage = "*")]
		[FutureDate(ErrorMessage = "Du måste ange ett datum i framtiden")]
		public DateTime Date { get; set; }

		[Display(Name = "Rubrik")]
		[Required(ErrorMessage = "*")]
		public string Name { get; set; }

		[Display(Name = "Typ")]
		[Required(ErrorMessage = "*")]
		public ActivityType Type { get; set; }

		[Display(Name = "Publik information")]
		public string Summary { get; set; }

		public ContributedInventoryModel Items { get; set; }

		[Display(Name = "Pass")]
		[Required(ErrorMessage = "*")]
		public SessionModel Session { get; set; }

		public CreateActivityModel()
		{
			this.Items = new ContributedInventoryModel();
		}
	}
}