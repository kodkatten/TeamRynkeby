namespace EventBooking.Data.Repositories
{
	public interface IMailTemplateRepository
	{
		string GetByName(string templateName);
	}
}