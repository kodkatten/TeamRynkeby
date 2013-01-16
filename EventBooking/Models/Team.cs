using System.Collections.Generic;

namespace EventBooking.Models
{
    public class Team
    {
        public string Name { get; set; }
        public ICollection<User> Volunteers { get; set; }
    }
}