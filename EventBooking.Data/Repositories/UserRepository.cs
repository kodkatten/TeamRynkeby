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
		    var databaseUser = context.Users.Find(user.Id);
		    databaseUser.Name = user.Name;
		    databaseUser.StreetAddress = user.StreetAddress;
		    databaseUser.Team = context.Teams.Find(user.Team.Id);
		    databaseUser.Zipcode = user.Zipcode;
		    databaseUser.Birthdate = user.Birthdate;
		    databaseUser.Cellphone = user.Cellphone;
		    databaseUser.City = user.City;
			context.SaveChanges();
		}

		public bool Exists(string email)
		{
			return context.Users.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
		}
	}
}