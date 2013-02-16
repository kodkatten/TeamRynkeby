using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class UpcomingActivitiesModel
    {
        public IEnumerable<ActivityModel> Activities { get; set; }
        
        public UpcomingActivitiesModel(IEnumerable<Activity> activityData)
        {
            this.Activities = activityData.Select(data => new ActivityModel(data));
        }
    }
}