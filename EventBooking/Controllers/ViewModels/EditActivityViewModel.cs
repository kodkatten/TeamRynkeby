using System.Collections.Generic;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class EditActivityViewModel
    {
        public ICollection<Session> Sessions { get; set; }
        public Activity Activity { get; set; }
        public List<string> ActivityTypes { get; set; }
        public ActivityType SelectedActivity { get; set; }
    }
}