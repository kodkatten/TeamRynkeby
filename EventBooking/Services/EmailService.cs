using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;
using EventBooking.Settings;

namespace EventBooking.Services
{
	public class EmailService : IEmailService
	{
		public enum EmailType
		{
			None = 0,
			NewActivity,
			InfoActivity
		}

		private readonly EmailSettings _emailSettings;
		private readonly IMailTemplateService _templateService;
		private readonly IActivityRepository _activityRepository;
		private readonly ITeamRepository _teamRepository;
		private readonly MailMessage _mailMessage;

		public EmailService(EmailSettings emailSettings, IMailTemplateService templateService, IActivityRepository activityRepository, ITeamRepository teamRepository)
		{
			_emailSettings = emailSettings;
			_templateService = templateService;
			_activityRepository = activityRepository;
			_teamRepository = teamRepository;

			_mailMessage = new MailMessage();
		}

		public void SendMail(int activityId, EmailType emailType, string freeText = "")
		{
			var activity = _activityRepository.GetActivityById(activityId);
			var teamId = activity.OrganizingTeam.Id;
			var teamMembers = _teamRepository.GetTeamMembers(teamId);
			var toAddressToName = teamMembers.ToDictionary(teamMember => teamMember.Email, teamMember => teamMember.Name);

			var text = NewEventText(activity, emailType, freeText);
			SendMail(toAddressToName, _emailSettings.From, activity.OrganizingTeam.Name, text.Subject, text.Body);
		}

		public MailData GetPreview(int activityId, EmailType emailType, string freeText)
		{
			var activity = _activityRepository.GetActivityById(activityId);
			return NewEventText(activity, emailType, freeText);
		}

		private MailData NewEventText(Activity activity, EmailType emailType, string freeText)
		{
			var templateName = "";
			switch (emailType)
			{
				case EmailType.None:
					return null;
				case EmailType.NewActivity:
					templateName = "newactivity";
					break;
				case EmailType.InfoActivity:
					templateName = "infoactivity";
					break;
			}

			var data = new Dictionary<string, object>
		        {
					{"ActivityName", activity.Name},
			        {"Team", activity.OrganizingTeam.Name},
					{"Date", activity.Date.ToString("yyyy-MM-dd")},
					{"Summary", activity.Summary},
					{"Description", activity.Description},
					{"ActivityManager", activity.Coordinator.Name}
		        };

			if (activity.Sessions != null)
			{
				data.Add("FirstTime", activity.Sessions.OrderBy(s => s.FromTime).First().FromTime.ToString("hh':'mm"));
				data.Add("LastTime", activity.Sessions.OrderByDescending(s => s.ToTime).First().ToTime.ToString("hh':'mm"));

				var usersAndPasses = activity.Sessions.SelectMany(s => s.Volunteers).Distinct().OrderBy(u => u.Name).Select(u => new
					{
						u.Name,
						u.Cellphone,
                        Sessions = u.Sessions.Where(s => s.Activity != null && s.Activity.Id == activity.Id).OrderBy(s => s.FromTime).Select(s => new
							{
								FromTime = s.FromTime.ToString("hh':'mm"),
								ToTime = s.ToTime.ToString("hh':'mm")								
							})
					});
				data.Add("Users", usersAndPasses);
			}

			if (!string.IsNullOrWhiteSpace(freeText))
			{
				data.Add("FreeText", freeText.Replace("\n", "<br/>"));
			}

			return _templateService.RenderTemplate(templateName, data);
		}

		private void SendMail(Dictionary<string, string> toAddressToName, string fromAddress, string fromName, string subject, string text)
		{
			foreach (var nameAddress in toAddressToName)
			{
				_mailMessage.To.Add(new MailAddress(nameAddress.Key, nameAddress.Value));
			}

			Send(fromAddress, fromName, subject, text);
		}

		private void Send(string fromAddress, string fromName, string subject, string text)
		{
			_mailMessage.From = new MailAddress(fromAddress, fromName);

			_mailMessage.Subject = subject;
			_mailMessage.Body = text;
			_mailMessage.IsBodyHtml = true;

			var smtpClient = new SmtpClient(_emailSettings.Server, Convert.ToInt32(_emailSettings.Port));
			var credentials = new System.Net.NetworkCredential(_emailSettings.User,_emailSettings.Password);
			smtpClient.Credentials = credentials;

			try
			{
				smtpClient.Send(_mailMessage);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message.ToString(CultureInfo.InvariantCulture));
			}
		}
	}
}