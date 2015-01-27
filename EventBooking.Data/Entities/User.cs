using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EventBooking.Data.Entities
{
   public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Cellphone { get; set; }	
        public DateTime Created { get; set; }
		public virtual Team Team { get; set; }
		public virtual ICollection<Session> Sessions { get; set; }
		

		public User()
		{
            Sessions = new Collection<Session>();
		}

		public bool IsMemberOfATeam()
		{
			return this.Team != null;
		}
	}
}