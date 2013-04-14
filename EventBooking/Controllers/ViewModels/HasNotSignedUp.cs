using System.Collections.Generic;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class HasNotSignedUp
    {
        public IEnumerable<User> Users { get; set; }
        public int ActivityId { get; set; }
    }
}