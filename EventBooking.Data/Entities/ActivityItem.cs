using System;
using System.Collections.Generic;

namespace EventBooking.Data.Entities
{
    public interface IActivityItem
    {
        int Id { get; set; }
        string Name { get; set; }
        int Quantity { get; set; }
        Activity Activity { get; set; }

    }
	public class ActivityItem : IActivityItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public virtual Activity Activity { get; set; }
	}

    public class ActivityItemComparer : IEqualityComparer<IActivityItem>
    {
        public bool Equals(IActivityItem x, IActivityItem y)
        {
            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal. 
            return x.Name== y.Name && x.Quantity == y.Quantity;
        }

        public int GetHashCode(IActivityItem obj)
        {
            throw new NotImplementedException();
        }
    }

    
}
