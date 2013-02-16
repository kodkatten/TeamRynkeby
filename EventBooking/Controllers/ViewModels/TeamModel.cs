using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamModel
    {
        public TeamModel(Team team)
        {
            this.Name = team.Name;
        }

        public string Name { get; set; }
    }
}