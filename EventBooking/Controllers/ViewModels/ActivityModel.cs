using System;
using System.ComponentModel.DataAnnotations;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class ActivityModel
	{
		public ActivityModel(Activity activityData)
		{
            if (activityData == null)
            {
                return;
            }

			Id = activityData.Id;
			Name = activityData.Name;
			OrganizingTeam = activityData.OrganizingTeam == null ? "" : activityData.OrganizingTeam.Name;
			DateFormatted = activityData.Date.ToSwedishShortDateString();
			Summary = activityData.Summary;
		}

	    public ActivityModel()
	    {
	        
	    }
		public int Id { get; private set; }

		[Display(Name = "Datum")]
		public string DateFormatted { get; private set; }

		[Display(Name = "Namn")]
		public string Name { get; private set; }

		public string OrganizingTeam { get; private set; }

        public string Summary { get; private set; }

        //public string Url { get; private set; }

	}
}