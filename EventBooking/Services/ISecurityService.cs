using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBooking.Data;
using WebMatrix.WebData;

namespace EventBooking.Services
{
    public interface ISecurityService
    {
        User GetUser(string userName, string password);
        void SignOff();
    }

    public class SecurityService : ISecurityService
    {
        public virtual User GetUser(string userName, string password)
        {
            if (!WebSecurity.Login(userName, password))
                return null;

            var userId = WebSecurity.GetUserId(userName);
            using (var context = new EventBookingContext())
            {
                return context.Users.Find(userId);
            }
        }

        public void SignOff()
        {
            WebSecurity.Logout();
        }
    }

    public class MockupSecurityService : SecurityService
    {
        public override User GetUser(string userName, string password)
        {
            if (userName == AcceptedEmail && password == AcceptedPassword)
                return new User();
            return null;
        }

        public string AcceptedEmail { get; set; }

        public string AcceptedPassword { get; set; }
    }
}
