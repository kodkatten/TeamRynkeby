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
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
			int userId = WebSecurity.GetUserId(userName);
			using (var context = new EventBookingContext())
			{
				return context.Users.Where( u => u.Id == userId ).Include( t => t.Team ).First();
			}
		}

		public virtual bool SignIn(string userName, string password)
		{
			if (WebSecurity.Login(userName, password))
			{
			    return true;
			}
		    return false;
		}

		public virtual void CreateUserAndAccount(string email, string password, DateTime created)
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
            get { return WebSecurity.CurrentUserName != null ? GetUser(WebSecurity.CurrentUserName) : null; }
		}

		public virtual bool IsLoggedIn
		{
			get { return WebSecurity.IsAuthenticated; }
		}
	}
}