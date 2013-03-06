using System;
using System.Collections.Generic;
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

		
		public bool CanCurrentUserManageTeams()
		{
			return IsCurrentUserPowerUser() || IsCurrentUserAdministrator();

		}

		public bool IsCurrentUserPowerUser()
		{
			var roles = (SimpleRoleProvider)Roles.Provider;
			return roles.GetRolesForUser(GetCurrentUser().Email).Contains(UserType.PowerUser.ToString());
		}

		public bool ToogleTeamPowerUser(int userId, int teamId)
		{
			var targetUser = GetUserOrThrow(userId);
			var team = GetTeamOrThrow(teamId);


			var teamPowerUserRole = GetPowerUserRoleForTeam(team);
			var isInRole = Roles.IsUserInRole(targetUser.Email, teamPowerUserRole);
			if (isInRole)
			{
				Roles.RemoveUserFromRole(targetUser.Email, teamPowerUserRole);
				return false;
			}

			Roles.AddUserToRole(targetUser.Email, teamPowerUserRole);
			return true;
		}

		bool ISecurityService.IsCurrentUserAdministrator()
		{
			return IsCurrentUserAdministrator();
		}

		public bool IsCurrentUserAdministratorOrPowerUser()
		{
			return Roles.GetAllRoles()
				.Any(x => x.Contains(UserType.PowerUser.ToString()) || x == UserType.Administrator.ToString());
		}

		public bool IsCurrentUserAdministratorOrPowerUser(int teamId)
		{
			var team = GetTeamOrThrow(teamId);
			var teamPowerUserRoleName = GetPowerUserRoleForTeam(team);
			return Roles.GetAllRoles()
				.Any(x => x == teamPowerUserRoleName || x == UserType.Administrator.ToString());
		}

		public string GetPowerUserRoleForTeam(Team team)
		{
			return team.Name + " PowerUser";
		}

		public bool ToogleAdministrator(int userId)
		{
			var targetUser = GetUserOrThrow(userId);
			var isInRole = Roles.IsUserInRole(targetUser.Email, UserType.Administrator.ToString());
			if (isInRole)
			{
				Roles.RemoveUserFromRole(targetUser.Email, UserType.Administrator.ToString());
				return false;
			}

			Roles.AddUserToRole(targetUser.Email, UserType.Administrator.ToString());
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

		private bool IsCurrentUserAdministrator()
		{
			var roles = (SimpleRoleProvider)Roles.Provider;
			var user = GetCurrentUser();

			if(user == null)
				throw new ArgumentException("Unknown user");
			
			return roles.GetRolesForUser(user.Email).Contains(UserType.Administrator.ToString());
		}

		private Team GetTeamOrThrow(int teamId)
		{
			var team = _context.Teams.FirstOrDefault(x => x.Id == teamId);

			if (team == null)
			{
				throw new ArgumentException("Unknown team");
			}
			return team;
		}

		private User GetUserOrThrow(int userId)
		{
			var targetUser = _context.Users.FirstOrDefault(x => x.Id == userId);

			if (targetUser == null)
			{
				throw new ArgumentException("Unknown user");
			}
			return targetUser;
		}
	}
}