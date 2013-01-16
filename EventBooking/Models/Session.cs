using System;
using System.Collections.Generic;

namespace EventBooking.Models
{
    public class Session
    {
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public Activity Activity { get; set; }
        public ICollection<User> Volunteers { get; set; } 
    }
}