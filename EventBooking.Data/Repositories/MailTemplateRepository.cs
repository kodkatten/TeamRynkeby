using System;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
	public class MailTemplateRepository : IMailTemplateRepository
	{
		private readonly IEventBookingContext _context;

		public MailTemplateRepository(IEventBookingContext context)
		{
			_context = context;
		}

		public MailTemplate GetByName(string templateName)
		{
			return _context.MailTemplates.FirstOrDefault(mt => mt.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase));			
		}
	}
}