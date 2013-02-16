using System.Collections.Generic;

namespace EventBooking.Data
{
    public class Team
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public virtual ICollection<User> Volunteers { get; set; }
        
        public virtual ICollection<Activity> Activities { get; set; } 
    }
}