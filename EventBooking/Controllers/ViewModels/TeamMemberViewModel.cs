namespace EventBooking.Controllers.ViewModels
{
    public class TeamMemberViewModel
    {
        public System.Linq.IQueryable<Data.Entities.User> TeamMembers { get; set; }
    }
}