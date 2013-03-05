using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EventBooking.Data
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

		public virtual ICollection<Team> AdminInTeams { get; set; }   
		public virtual ICollection<Session> Sessions { get; set; }
		public virtual ICollection<UserActivityItem> Items { get; set; }

		public User()
		{
			AdminInTeams = new Collection<Team>();
			Items = new Collection<UserActivityItem>();
			Sessions = new Collection<Session>();
		}

		public bool IsMemberOfATeam()
		{
			return this.Team != null;
		}

		public bool IsAdminForTeam(int teamId)
		{
			return AdminInTeams.Any(x => x.Id == teamId);
		}
	}
}