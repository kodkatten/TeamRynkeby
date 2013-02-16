using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public bool IsTeamAdmin { get; set; }
	}
}