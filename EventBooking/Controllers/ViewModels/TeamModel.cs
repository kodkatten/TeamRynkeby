using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamModel
    {
        public TeamModel(Team team)
        {
            this.Name = team.Name;
            this.Id = team.Id;
        }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}