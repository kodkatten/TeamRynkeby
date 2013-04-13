using System.Collections.Generic;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class SignedForActivityViewModel : DetailActivityViewModel
    {
        public SignedForActivityViewModel(Activity activityData, User user) : base(activityData, user)
        {
            Session = activityData.Sessions;
        }

        public IEnumerable<Session> Session { get; set; }
    }
}