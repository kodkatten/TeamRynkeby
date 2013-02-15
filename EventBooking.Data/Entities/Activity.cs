using System;
using System.Collections.Generic;

namespace EventBooking.Data
{
    public class Activity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public ActivityType Type { get; set; }
        public User Coordinator { get; set; }
        public DateTime Date { get; set; }
		public Team OrganizingTeam { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<Item> RequiredItems { get; set; }
    }
}