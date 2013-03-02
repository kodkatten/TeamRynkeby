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
		bool IsUserTeamAdmin();
	}
}