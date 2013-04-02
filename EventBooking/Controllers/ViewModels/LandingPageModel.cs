using System.Collections.Generic;
using EventBooking.Data;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class  LandingPageModel
    {
        public bool IsNobody { get; set; }
        public int SelectedTeamId { get; set; }
        public IEnumerable<Activity> Activities { get; private set; }
        public IEnumerable<Team> Teams { get; private set; } 

        public LandingPageModel()
        {
            Activities = new Activity[] { };
            Teams = new Team[] { };
        }

        public LandingPageModel(IEnumerable<Activity> activities, IEnumerable<Team> teams)
        {
            Activities = activities;
            Teams = teams;
        }
    }

}