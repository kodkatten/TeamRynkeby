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
}
