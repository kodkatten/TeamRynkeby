using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
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

		public virtual bool IsCurrentUserTeamAdminFor(int teamId)
		{
			var loggedOnUser = GetCurrentUser();
			var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
			
			return loggedOnUser != null
			       && team != null
			       && team.TeamAdmins.Contains(loggedOnUser);
		}

		public bool IsCurrentUserAdminOfAnyTeam()
		{
			var loggedOnUser = GetCurrentUser();

			if (loggedOnUser == null)
				return false;

			return _context.Teams.FirstOrDefault(t => t.TeamAdmins.FirstOrDefault(u => u.Id == loggedOnUser.Id) != null) != null;
		}

		public bool IsCurrentUserPowerUser()
		{
			var roles = (SimpleRoleProvider)Roles.Provider;
			return roles.GetRolesForUser(GetCurrentUser().Email).Contains(UserType.PowerUser.ToString());
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