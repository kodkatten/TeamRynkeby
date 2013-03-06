using System;
using EventBooking.Data;

namespace EventBooking.Services
{
	public interface ISecurityService
	{
		bool IsLoggedIn();
		User GetCurrentUser();
		void SignOff();
		bool SignIn(string username, string password);
		void CreateUserAndAccount(string email, string password, DateTime created);
	    User GetUser(string electronicMailAddress);
		bool ToogleTeamPowerUser(int userId, int teamId);
		bool IsCurrentUserAdministrator();
		bool IsCurrentUserAdministratorOrPowerUser();
		bool IsCurrentUserAdministratorOrPowerUser(int teamId);
		string GetPowerUserRoleForTeam(Team team);
		bool ToogleAdministrator(int userId);
	}
}