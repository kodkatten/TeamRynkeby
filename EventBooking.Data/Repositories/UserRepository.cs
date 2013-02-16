using System;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	internal class UserRepository : IUserRepository
	{
		private readonly IEventBookingContext context;

		public UserRepository(IEventBookingContext context)
		{
			this.context = context;
		}

		public void Save(User user)
		{
			context.Users.Attach(user);
			context.SaveChanges();
		}

		public bool Exists(string email)
		{
			return context.Users.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
		}
	}
}