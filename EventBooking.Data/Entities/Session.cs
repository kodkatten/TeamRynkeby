using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Entities
{
	public class Session
	{
		public int Id { get; set; }
		public TimeSpan FromTime { get; set; }
		public TimeSpan ToTime { get; set; }
		public Activity Activity { get; set; }
		public ICollection<User> Volunteers { get; set; }
		public int VolunteersNeeded { get; set; }

		public bool IsAllowedToSignUp(User user)
		{
			return user != null &&
				   this.Volunteers.Count < this.VolunteersNeeded &&
				   this.Activity.OrganizingTeam.Id == user.Team.Id &&
				   this.Volunteers.All(volunteer => volunteer.Id != user.Id);
		}

		public bool CanLeave(User user)
		{
			return Volunteers.Any(v => v.Id == user.Id);
		}

		//public void SignUp(User user)
		//{
		//	if (!this.IsAllowedToSignUp(user))
		//	{
		//		throw new Exception("This peep is not allowed to sign up for the session!");
		//	}

		//	Volunteers.Add(user);
		//	// user.Sessions.Add(this); /* why not? */
		//}
	}
}