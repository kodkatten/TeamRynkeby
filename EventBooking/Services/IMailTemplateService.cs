using System.Collections.Generic;
using EventBooking.Controllers.ViewModels;

namespace EventBooking.Services
{
	public interface IMailTemplateService
	{
		MailData RenderTemplate(string templateName, IDictionary<string, object> data);
	}
}