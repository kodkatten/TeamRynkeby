using System.Collections.Generic;

namespace EventBooking.Controllers.ViewModels
{
    public class AdministerTeamMembersModel
    {
        public int TeamId { get; set; }
        public IEnumerable<Volunteer> Type { get; set; }
    }
}