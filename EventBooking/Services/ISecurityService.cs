using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBooking.Data;

namespace EventBooking.Services
{
    public interface ISecurityService
    {
        User GetUser(string userName, string password);
    }

    public class SecurityService : ISecurityService
    {
        public virtual User GetUser(string userName, string password)
        {
            throw new NotImplementedException();
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
