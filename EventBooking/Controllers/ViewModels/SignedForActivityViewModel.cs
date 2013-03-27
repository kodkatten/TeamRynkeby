using System.Collections.Generic;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class SignedForActivityViewModel : DetailActivityViewModel
    {
        public SignedForActivityViewModel(Activity activityData, User user)
            : base(activityData, user)
        {}

        public IEnumerable<Session> Session { get; set; }

        public IEnumerable<ActivityItem> ActivityItems { get; set; }
       
    }
}