using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;

namespace EventBooking.email
{
    public class Email
    {

        public void SendMail(string toAddress, string toName, string fromAddress, string fromName, string subject, string text)
        {

            var mailMsg = new MailMessage();

            mailMsg.To.Add(new MailAddress(toAddress, toName));
            mailMsg.From = new MailAddress(fromAddress, fromName);

            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            
            Send(mailMsg);
           
        }
        public void SendMail(Dictionary<string, string> toAddressToName, string fromAddress, string fromName, string subject, string text)
        {

            var mailMsg = new MailMessage();

            foreach (var nameAddress in toAddressToName)
            {
                mailMsg.To.Add(new MailAddress(nameAddress.Value, nameAddress.Key));
            }

            mailMsg.From = new MailAddress(fromAddress, fromName);

            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));

            // Init SmtpClient and send
            var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            var credentials = new System.Net.NetworkCredential("b49606c3-69d5-4ed8-975f-78ec56fe6a84@apphb.com", "byu9cpgi");
            smtpClient.Credentials = credentials;

            smtpClient.Send(mailMsg);
        }
        private void Send(MailMessage mailMsg)
        {
            
            var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            var credentials = new System.Net.NetworkCredential("b49606c3-69d5-4ed8-975f-78ec56fe6a84@apphb.com", "byu9cpgi");
            smtpClient.Credentials = credentials;

            smtpClient.Send(mailMsg); ;
        }

    }
}