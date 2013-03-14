using System.Collections.Generic;
using System.IO;
using EventBooking.Data.Repositories;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace EventBooking.Services
{
	public class MailTemplateService
	{
		private readonly IMailTemplateRepository _mailTemplateRepository;

		public MailTemplateService(IMailTemplateRepository mailTemplateRepository)
		{
			_mailTemplateRepository = mailTemplateRepository;
		}

		public string RenderTemplate(string templateName, IDictionary<string, object> data)
		{
			string templateContent = GetTemplateContent(templateName);
			var engine = new VelocityEngine();
			engine.Init();
			
			var context = GetContext(data);

			using (var writer = new StringWriter())
			{
				engine.Evaluate(context, writer, "", templateContent);
				return writer.GetStringBuilder().ToString();
			}
		}

		private static IContext GetContext(IEnumerable<KeyValuePair<string, object>> data)
		{
			var context = new VelocityContext();
			foreach (var d in data)
			{
				context.Put(d.Key, d.Value);
			}

			return context;
		}

		private string GetTemplateContent(string templateName)
		{
			return _mailTemplateRepository.GetByName(templateName);
		}
	}
}