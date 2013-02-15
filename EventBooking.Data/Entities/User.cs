using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Data
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string StreetAddress { get; set; }
        public int? Zipcode { get; set; }
        public string City { get; set; }
        public DateTime Created { get; set; }
        public Team Team { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}