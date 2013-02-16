using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Data.Repositories
{
	public sealed class PrefedinedItemRepository : IPrefedinedItemRepository
	{
		private readonly IEventBookingContext _context;

		public PrefedinedItemRepository(IEventBookingContext context)
		{
			_context = context;
		}

		public IQueryable<PredefinedActivityItem> GetPredefinedActivityItems()
		{
			return _context.PredefinedActivityItems;
		}
	}
}
