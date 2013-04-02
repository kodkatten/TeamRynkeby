using System;

namespace EventBooking.Data.Entities
{
    [Obsolete]
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Activity Activity { get; set; }
        public User User { get; set; }
    }
}