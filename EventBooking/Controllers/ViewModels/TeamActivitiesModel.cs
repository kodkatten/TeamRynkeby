using System;
using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamActivitiesModel
    {
        public DateTime StartDate { get; private set; }

        public TeamActivitiesModel(Team team, User user, DateTime startDate)
        {
            StartDate = startDate;
            var activities = (team.Activities ?? new Activity[0]);
            Activities = activities.Where(a => startDate <= a.Date).OrderBy(r=>r.Date).GroupBy(activity => activity.Date.ToString("MMMM")).Take(2);
            Name = team.Name;
            MyActivites = activities.Where(r=>r.Sessions.Any(s=>s.Volunteers.Any(v=>v.Id == user.Id) || r.Coordinator != null && r.Coordinator.Id == user.Id)).OrderBy(t=>t.Date);
        }

        public IEnumerable<Activity> MyActivites { get; private set;} 

        public IEnumerable<IGrouping<string, Activity>> Activities { get; internal set; }

        public string Name { get; internal set; }
    }
}