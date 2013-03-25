using System.Collections.Generic;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class SignedForActivity : DetailActivityViewModel
    {
        public SignedForActivity(Activity activityData, User user)
            : base(activityData, user)
        {}

        public IEnumerable<Session> Session { get; set; }
       
    }
}