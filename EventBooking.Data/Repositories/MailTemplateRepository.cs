using System;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public class MailTemplateRepository : IMailTemplateRepository
	{
		private readonly IEventBookingContext _context;

		public MailTemplateRepository(IEventBookingContext context)
		{
			_context = context;
		}

		public string GetByName(string templateName)
		{
			var template = _context.MailTemplates.FirstOrDefault(mt => mt.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase));
			return template == null ? string.Empty : template.Content;
		}
	}
}