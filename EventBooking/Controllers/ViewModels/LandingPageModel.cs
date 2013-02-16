using System.Collections.Generic;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class  LandingPageModel
    {
        public bool IsNobody { get; set; }
        public int SelectedTeamId { get; set; }
        public IEnumerable<Activity> Activities { get; private set; } 

        public LandingPageModel()
        {
            Activities = new Activity[] { };
        }


        public LandingPageModel(IEnumerable<Activity> activities )
        {
            Activities = activities;
        }
    }

}