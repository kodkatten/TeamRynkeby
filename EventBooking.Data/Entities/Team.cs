using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EventBooking.Data.Entities
{
    public class Team
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }

	    public virtual ICollection<User> Volunteers { get; set; }
		public virtual ICollection<Activity> Activities { get; set; }

	    public Team()
	    {
			Volunteers = new Collection<User>();
			Activities = new Collection<Activity>();
	    }
    }
}