using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamActivitiesModel
    {
        public IEnumerable<IGrouping<string, Data.Activity>> Activities { get; internal set; }

        public string Name { get; internal set; }
    }
}