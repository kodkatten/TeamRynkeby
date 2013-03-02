using System;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data;
using WebMatrix.WebData;

namespace EventBooking.Services
{
	public class SecurityService : ISecurityService
	{
		private readonly IEventBookingContext _context;

		public SecurityService(IEventBookingContext context)
		{
			_context = context;
		}

		public virtual User GetUser(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName))
			{
				return null;
			}

			// We think that we're logged in, but we're really not.
			int userId = WebSecurity.GetUserId(userName);
			bool isLoggedIn = this.IsLoggedIn();
			if (userId == -1 && isLoggedIn)
			{
				// Logout
				WebSecurity.Logout();
			}

			// Find the user.
			return _context.Users.Where(u => u.Id == userId).Include(t => t.Team).FirstOrDefault();
		}

		public virtual User GetCurrentUser()
		{
			return WebSecurity.CurrentUserName != null ? GetUser(WebSecurity.CurrentUserName) : null;
		}

		public virtual bool IsUserTeamAdmin()
		{
			return true;
		}

		public virtual bool IsLoggedIn()
		{
			return WebSecurity.IsAuthenticated && WebSecurity.CurrentUserId != -1;
		}

		public virtual bool SignIn(string userName, string password)
		{
			return WebSecurity.Login(userName, password);
		}

		public virtual void CreateUserAndAccount(string email, string password, DateTime created)
		{
			// Create the account.
			DateTime earlier = DateTime.UtcNow;
			string confirmationToken = WebSecurity.CreateUserAndAccount(email, password, new { Created = earlier });

			// Create the user.
			User user = new User();
			user.Email = email;
			user.Id = WebSecurity.GetUserId(email);
			user.Created = earlier;

			// Save the user.
			_context.Users.Add(user);
			_context.SaveChanges();
		}

		public void SignOff()
		{
			WebSecurity.Logout();
		}
	}
}