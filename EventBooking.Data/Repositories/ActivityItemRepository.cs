using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
