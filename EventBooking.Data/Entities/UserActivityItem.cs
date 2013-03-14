namespace EventBooking.Data.Entities
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
