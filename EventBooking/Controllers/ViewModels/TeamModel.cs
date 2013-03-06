using System.Collections.Generic;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class TeamModel
    {
	    public int Id { get; set; }
	    public string Name { get; set; }
	    public string PowerUserRole { get; set; }
	    public IEnumerable<UserModel> Volunteers { get; set; }
    }
}