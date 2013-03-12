using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public sealed class ActivityItemRepository : IActivityItemRepository
	{
		private readonly IEventBookingContext _context;

		public ActivityItemRepository(IEventBookingContext context)
		{
			_context = context;
		}

		public IQueryable<ActivityItemTemplate> GetTemplates()
		{
			return _context.ActivityItemTemplates;
		}

		public IEnumerable<ActivityItem> GetItemsForActivity(int activityId)
		{
			return _context.ActivityItems.Where(i => i.Activity.Id == activityId).Include(i => i.Activity).ToArray();
		}

		public void Save(int activityId, ActivityItem item)
		{
			item.Activity = _context.Activities.FirstOrDefault(x => x.Id == activityId);
			_context.ActivityItems.Add(item);
			_context.SaveChanges();
		}

		public ActivityItem GetItem(int itemId)
		{
			return _context.ActivityItems.FirstOrDefault(i => i.Id == itemId);
		}

		public void UpdateItem(int activityId, ActivityItem item)
		{
			ActivityItem itemToUpdate = _context.ActivityItems.FirstOrDefault(i => i.Activity.Id == activityId && i.Name == item.Name);
			
			if (itemToUpdate != null)
			{
				itemToUpdate.Quantity = item.Quantity;				
				_context.SaveChanges();	
			}
		}

		public void DeleteItem(int itemId)
		{
			ActivityItem itemToDelete = _context.ActivityItems.First(i => i.Id == itemId);

			_context.ActivityItems.Remove(itemToDelete);
			_context.SaveChanges();
		}

		public void DeleteItemByActivityIdAndItemName(int activityId, string itemName)
		{
			ActivityItem itemToDelete = GetItemByActivityIdAndItemName(activityId, itemName);

			_context.ActivityItems.Remove(itemToDelete);
			_context.SaveChanges();
		}

		public ActivityItem GetItemByActivityIdAndItemName(int activityId, string itemName)
		{
			return _context.ActivityItems.FirstOrDefault(i => i.Activity.Id == activityId && i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
		}

		public void AddOrUpdateItem(int activityId, string itemName, int itemQuantity)
		{
			var existing = GetItemByActivityIdAndItemName(activityId, itemName);
			
			if (existing == null)
			{
				existing = new ActivityItem
					{
						Activity = _context.Activities.FirstOrDefault(x => x.Id == activityId),
						Name = itemName,
						Quantity = itemQuantity
					};
				_context.ActivityItems.Add(existing);
			}
			else
			{
				existing.Quantity += itemQuantity;
			}

			_context.SaveChanges();
		}
	}
}
