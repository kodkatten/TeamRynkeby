using System.Collections.Generic;

namespace EventBooking.Controllers.ViewModels
{
    public class  LandingPage
    {
        public IEnumerable<Activity> Activities { get; internal set; }
    }
}