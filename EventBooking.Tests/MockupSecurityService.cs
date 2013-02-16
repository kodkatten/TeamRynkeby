using EventBooking.Data;
using EventBooking.Services;

namespace EventBooking.Tests
{
	public class MockupSecurityService : SecurityService
	{
	    public string AcceptedEmail { get; set; }

		public string AcceptedPassword { get; set; }

		public override bool IsLoggedIn
		{
			get { return null != CurrentUser; }
		}

		public User ReturnUser { get; set; }

		public override User CurrentUser { get { return ReturnUser; } }

		public override void CreateUserAndAccount( string email, string password, System.DateTime created )
		{
			
		}

		public override User GetUser( string userName )
		{
			return ReturnUser;
		}

		public override bool SignIn(string userName, string password)
		{
			if (userName == AcceptedEmail && password == AcceptedPassword)
			{
			    ReturnUser = ReturnUser ?? new User {Email = userName};
				return true;
			}
			return false;
		}
	}
}