using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class UpcomingActivitiesModel
    {
        public IEnumerable<ActivityModel> Activities { get; set; }

        public string NextUrl { get; set; }

        public string PreviousUrl { get; set; }
        
        public UpcomingActivitiesModel(IEnumerable<Activity> activityData)
        {
            this.Activities = activityData.Select(data => new ActivityModel(data));
            this.NextUrl = "http://www.example.com";
            this.PreviousUrl = "http://www.example.com";
        }
    }
}