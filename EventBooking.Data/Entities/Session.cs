using System;
using System.Collections.Generic;

namespace EventBooking.Data
{
    public class Session
    {
        public int Id { get; set; }

        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public Activity Activity { get; set; }
        public ICollection<User> Volunteers { get; set; }
		public int VolunteersNeeded { get; set; }
    }
}