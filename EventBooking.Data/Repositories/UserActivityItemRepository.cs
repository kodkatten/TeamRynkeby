using System;
using EventBooking.Data.Entities;
using System.Linq;

namespace EventBooking.Data.Repositories
{
    public class UserActivityItemRepository : IUserActivityItemRepository
    {
        private readonly IEventBookingContext _context;

        public UserActivityItemRepository(IEventBookingContext context)
		{
			_context = context;
		}

        public void CreateOrUpdate(UserActivityItem item)
        {
            var itemToUpdate =
                _context.UserActivityItems.FirstOrDefault(i => i.Item.Id == item.Item.Id && i.User.Id == item.User.Id);
        
            if (itemToUpdate == null)
            {
                _context.UserActivityItems.Add(item);
            }
            else if (item.Quantity == 0)
            {
                Delete(itemToUpdate.Id);
            }
            else
            {
                itemToUpdate.Quantity = item.Quantity;
            }

            _context.SaveChanges();
        }

        public void Delete(int itemId)
        {
            UserActivityItem item = _context.UserActivityItems.FirstOrDefault(x => x.Id == itemId);
            _context.UserActivityItems.Remove(item);

            _context.SaveChanges();
        }
    }
}