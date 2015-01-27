using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventBooking.Data.Entities;
using EventBooking.Validators;

namespace EventBooking.Controllers.ViewModels
{
	public class CreateActivityModel
	{
		[Display(Name = "Rubrik")]
		[Required(ErrorMessage = "*")]
		public string Name { get; set; }

		[Display(Name = "Beskrivning")]
		[Required(ErrorMessage = "*")]
		public string Information { get; set; }

		
		[Display(Name = "Typ")]
		[Required(ErrorMessage = "*")]
		public ActivityType Type { get; set; }


        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Du måste ange ett datum")]
        [FutureDate(ErrorMessage = "Du måste ange ett datum i framtiden")]
        public DateTime Date { get; set; }

		
		[Display(Name = "Pass")]
		[Required(ErrorMessage = "Du måste ha minst ett pass")]
		public IList<SessionModel> Sessions { get; set; }


		public CreateActivityModel()
		{
            this.Sessions = new List<SessionModel>();
		}
	}
}