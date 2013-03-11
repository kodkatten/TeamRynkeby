namespace EventBooking.Data
{
	public class ActivityItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public virtual Activity Activity { get; set; }
	}
}
