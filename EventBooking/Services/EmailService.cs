using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Resources;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;

namespace EventBooking.Services
{
    public class EmailService : IEmailService
    {
        private readonly ActivityRepository _activityRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly MailMessage _mailMessage;

        public EmailService(ActivityRepository activityRepository, ITeamRepository teamRepository)
        {
            _activityRepository = activityRepository;
            _teamRepository = teamRepository;

            _mailMessage = new MailMessage();
        }

        public void SendMail(int activityId)
        {
            var activity = _activityRepository.GetActivityById(activityId);
            var teamId = activity.OrganizingTeam.Id;
            var teamMembers = _teamRepository.GetTeamMembers(teamId);
            var toAddressToName = teamMembers.ToDictionary(teamMember => teamMember.Email, teamMember => teamMember.Name);

            var text = NewEventText(activity);
            SendMail(toAddressToName, "noreply@teamrynkeby.apphb.com", activity.OrganizingTeam.Name, activity.Name, text);
        }

        private string NewEventText(Activity activity)
        {
            var resourceManger = new ResourceManager("EventBooking.Resources", Assembly.GetExecutingAssembly());
            var template = resourceManger.GetString("Template_NewEvent");

            if (template == null)
                throw new Exception("NewEventTextNoResource");

            template = template.Replace("[Team]", activity.OrganizingTeam.ToString());

            var date = activity.Date.Year + "-" + activity.Date.Month + "-" + activity.Date.Day;
            template = template.Replace("[Datum]", date.ToString());
            template = template.Replace("[Summary]", activity.Summary.ToString());
            template = template.Replace("Description", activity.Description.ToString());

            return template;
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
            _mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));


            var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            var credentials = new System.Net.NetworkCredential("b49606c3-69d5-4ed8-975f-78ec56fe6a84@apphb.com",
                                                               "byu9cpgi");
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