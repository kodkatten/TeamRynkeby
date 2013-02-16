using System.Collections.Generic;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamModel
    {
	   

		public IEnumerable<UserModel> Volunteers { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}