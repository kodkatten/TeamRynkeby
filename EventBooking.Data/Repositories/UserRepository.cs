using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
	internal class UserRepository : IUserRepository
	{
		private readonly IEventBookingContext _context;

		public UserRepository(IEventBookingContext context)
		{
			this._context = context;
		}

		public void Save(User user)
		{
			// Get the user from the database.
			var databaseUser = _context.Users.First(u => u.Id == user.Id);
			// Get the user's team from the database.
			var databaseTeam = _context.Teams.First(t => t.Id == user.Team.Id);

			databaseUser.Name = user.Name;
			databaseUser.StreetAddress = user.StreetAddress;
			databaseUser.Team = databaseTeam;
			databaseUser.Zipcode = user.Zipcode;
			databaseUser.Birthdate = user.Birthdate;
			databaseUser.Cellphone = user.Cellphone;
			databaseUser.City = user.City;

			// Save the user.
			_context.SaveChanges();
		}

		public void RemoveFromTeam(int userId)
		{
			var user = _context.Users.Include(u => u.Team).FirstOrDefault(x => x.Id == userId);
			if (user != null)
			{
				user.Team = null;
				_context.SaveChanges();
			}
		}

		public bool Exists(string email)
		{
			return _context.Users.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
		}
	}
}