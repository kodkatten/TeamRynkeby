using System;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Services
{
	public interface ISecurityService
	{
		bool IsLoggedIn { get; }
		User CurrentUser { get; }
		void SignOff();
		bool SignIn(string username, string password);
		void CreateUserAndAccount(string email, string password, DateTime created);
	    User GetUser(string electronicMailAddress);
	}
}