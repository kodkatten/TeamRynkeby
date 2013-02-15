using System.Collections.Generic;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamActivitiesModel
    {
        public TeamActivitiesModel(Team team)
        {
            Activities = team.Activities.GroupBy(activity => activity.Date.ToString("MMMM"));
            Name = team.Name;
        }

        public IEnumerable<IGrouping<string, Activity>> Activities { get; internal set; }

        public string Name { get; internal set; }
    }
}