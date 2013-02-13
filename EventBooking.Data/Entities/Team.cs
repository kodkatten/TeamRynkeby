using System.Collections.Generic;

namespace EventBooking.Data
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<User> Volunteers { get; set; }
    }
}