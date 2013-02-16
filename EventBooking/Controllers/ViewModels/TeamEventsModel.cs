using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamActivitiesModel
    {
        public TeamActivitiesModel(Team team, User user)
        {
            var activities = (team.Activities ?? new Activity[0]);
            Activities = activities.GroupBy(activity => activity.Date.ToString("MMMM"));
            Name = team.Name;
            MyActivites = activities.Where(r=>r.Sessions.Any(s=>s.Volunteers.Any(v=>v.Id == user.Id)));
        }

        public IEnumerable<Activity> MyActivites { get; private set;} 

        public IEnumerable<IGrouping<string, Activity>> Activities { get; internal set; }

        public string Name { get; internal set; }
    }
}