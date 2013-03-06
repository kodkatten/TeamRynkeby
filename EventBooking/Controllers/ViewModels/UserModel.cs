using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Controllers.ViewModels
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}