using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Data.Entities
{
   public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Cellphone { get; set; }
		public string StreetAddress { get; set; }
		public string Zipcode { get; set; }
		public string City { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Birthdate { get; set; }
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