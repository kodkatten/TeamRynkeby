using System;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Services;

namespace EventBooking.Tests
{
	public class MockupSecurityService : SecurityService
	{
		public string AcceptedEmail { get; set; }
		public string AcceptedPassword { get; set; }
		public User ReturnUser { get; set; }

		public MockupSecurityService()
			: base(null)
		{
		}

		public override bool IsLoggedIn()
		{
			return this.GetCurrentUser() != null;
		}

		public override User GetCurrentUser()
		{
			return ReturnUser;
		}

		public override void CreateUserAndAccount(string email, string password, DateTime created)
		{
		}

		public override User GetUser(string userName)
		{
			return ReturnUser;
		}

		public override bool SignIn(string userName, string password)
		{
			if (userName == AcceptedEmail && password == AcceptedPassword)
			{
				ReturnUser = ReturnUser ?? new User { Email = userName };
				return true;
			}

			return false;
		}
	}
}