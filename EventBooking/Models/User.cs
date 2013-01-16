using System;
using System.Collections.Generic;

namespace EventBooking.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string StreetAddress { get; set; }
        public int Zipcode { get; set; }
        public string City { get; set; }
        public DateTime Created { get; set; }
        public Team Team { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}