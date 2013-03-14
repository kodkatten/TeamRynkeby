using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
	public interface IMailTemplateRepository
	{
		MailTemplate GetByName(string templateName);
	}
}