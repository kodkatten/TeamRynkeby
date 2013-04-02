using System.Collections.Generic;
using System.IO;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace EventBooking.Services
{
	public class MailTemplateService : IMailTemplateService
	{
		private readonly IMailTemplateRepository _mailTemplateRepository;

		public MailTemplateService(IMailTemplateRepository mailTemplateRepository)
		{
			_mailTemplateRepository = mailTemplateRepository;
		}

		public MailData RenderTemplate(string templateName, IDictionary<string, object> data)
		{
			var template = GetTemplateContent(templateName);
			return RenderTemplateInternal(template, data);
		}

		private static MailData RenderTemplateInternal(MailTemplate template, IEnumerable<KeyValuePair<string, object>> data)
		{
			var engine = new VelocityEngine();
			engine.Init();

			var context = GetContext(data);

			var result = new MailData
				{
					Subject = EvaluateTemplate(engine, context, template.Subject),
					Body = EvaluateTemplate(engine, context, template.Content)
				};
			return result;
		}

		private static string EvaluateTemplate(VelocityEngine engine, IContext context, string templateString)
		{
			using (var writer = new StringWriter())
			{
				engine.Evaluate(context, writer, "", templateString);
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

		private MailTemplate GetTemplateContent(string templateName)
		{
			return _mailTemplateRepository.GetByName(templateName);
		}
	}
}