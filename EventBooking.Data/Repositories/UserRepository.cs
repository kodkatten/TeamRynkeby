using System;
using System.Data;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	class UserRepository : IUserRepository
	{
		private readonly IEventBookingContext context;

		public UserRepository(IEventBookingContext context)
		{
			this.context = context;
		}

		public bool Exists(string email)
		{
			return context.Users.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
		}

		public void Save(User user)
		{
			this.context.Users.Attach(user);
			if (user.Team != null)
				user.Team = context.Teams.Find(user.Team.Id);



			//this.context.Entry(user).State = EntityState.Modified;
			this.context.SaveChanges();
		}
	}
}