using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public interface IActivityItemRepository
	{
		IQueryable<ActivityItemTemplate> GetTemplates();
		IEnumerable<ActivityItem> GetItemsForActivity(int activityId);
		ActivityItem GetItem(int itemId);
		void Save(int activityId, ActivityItem item);
		void DeleteItem(int itemId);
		void UpdateItem(int activityId, ActivityItem item);
		void DeleteItemByActivityIdAndItemName(int activityId, string itemName);
		ActivityItem GetItemByActivityIdAndItemName(int activityId, string itemName);
		void AddOrUpdateItem(int activityId, string itemName, int itemQuantity);
	}
}
