using System;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data;
using WebMatrix.WebData;

namespace EventBooking.Services
{
	public class SecurityService : ISecurityService
	{
		public virtual User GetUser(string userName)
		{
			int userId = WebSecurity.GetUserId(userName);
			using (var context = new EventBookingContext())
			{
				return context.Users.Where( u => u.Id == userId ).Include( t => t.Team ).First();
			}
		}

		public virtual bool SignIn(string userName, string password)
		{
			return WebSecurity.Login(userName, password);
		}

		public void CreateUserAndAccount(string email, string password, DateTime created)
		{
			DateTime earlier = DateTime.UtcNow;

			WebSecurity.CreateUserAndAccount(email, password, new {Created = earlier});

			using (var context = new EventBookingContext())
			{
				context.Users.Add(new User
					{
						Email = email,
						Id = WebSecurity.GetUserId(email),
						Created = earlier
					});
				context.SaveChanges();
			}
		}

		public void SignOff()
		{
			WebSecurity.Logout();
		}

		public virtual User CurrentUser
		{
			get { return GetUser(WebSecurity.CurrentUserName); }
		}

		public virtual bool IsLoggedIn
		{
			get { return WebSecurity.IsAuthenticated; }
		}
	}
}