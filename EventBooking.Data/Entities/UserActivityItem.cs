using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Data
{
	public class UserActivityItem
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public virtual User User { get; set; }
		public virtual ActivityItem Item { get; set; }
		public int Quantity { get; set; }
	}
}
