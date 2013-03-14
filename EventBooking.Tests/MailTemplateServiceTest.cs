using System;
using System.Collections.Generic;
using System.Globalization;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Services;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EventBooking.Tests
{
	[TestFixture]
	public class MailTemplateServiceTest
	{
		[Test]
		public void When_Requesting_Template_With_Provided_Data_The_Template_Should_Be_Rendered()
		{
			var expected = string.Format("Team Rynkeby den {0} 1337 st", DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
			var template = new MailTemplate
				{
					Name = "template",
					Subject = "$Namn den $Datum $Antal st",
					Content = "$Namn den $Datum $Antal st"
				};
			var mailTemplateRepository = new Mock<IMailTemplateRepository>();
			mailTemplateRepository.Setup(m => m.GetByName("template")).Returns(template);
			var mailTemplateService = new MailTemplateService(mailTemplateRepository.Object);

			var data = new Dictionary<string, object>
				{
					{"Namn", "Team Rynkeby"},
					{"Datum", DateTime.MinValue.ToString(CultureInfo.InvariantCulture)},
					{"Antal", 1337}
				};

			var result = mailTemplateService.RenderTemplate("template", data);
			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result.Subject);
			Assert.AreEqual(expected, result.Body);
		}
	}
}