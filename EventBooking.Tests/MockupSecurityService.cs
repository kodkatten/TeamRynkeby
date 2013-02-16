using EventBooking.Data;
using EventBooking.Services;

namespace EventBooking.Tests
{
	public class MockupSecurityService : SecurityService
	{
		private bool _isLoggedin;

	    public string AcceptedEmail { get; set; }

		public string AcceptedPassword { get; set; }

		public override bool IsLoggedIn
		{
			get { return _isLoggedin; }
		}

		public User ReturnUser { get; set; }

		public override User CurrentUser { get { return ReturnUser; } }

		public override bool SignIn(string userName, string password)
		{
			if (userName == AcceptedEmail && password == AcceptedPassword)
			{
				_isLoggedin = true;
				return true;
			}
			return false;
		}
	}
}