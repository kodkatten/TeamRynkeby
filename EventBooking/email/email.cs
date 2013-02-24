using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Net.Mime;

namespace EventBooking.email
{
    public class Email
    {
        private readonly MailMessage _mailMessage;

        public Email()
        {
            _mailMessage = new MailMessage();
        }

        public  void SendMail(string toAddress, string toName, string fromAddress, string fromName, string subject, string text)
        {
            _mailMessage.To.Add(new MailAddress(toAddress, toName));
            _mailMessage.From = new MailAddress(fromAddress, fromName);

            Send(fromAddress, fromName, subject, text);

        }
        public  void SendMail(Dictionary<string, string> toAddressToName, string fromAddress, string fromName, string subject, string text)
        {
            foreach (var nameAddress in toAddressToName)
            {
                _mailMessage.To.Add(new MailAddress(nameAddress.Value, nameAddress.Key));
            }

            Send(fromAddress, fromName, subject, text);
        }

        private  void Send(string fromAddress, string fromName, string subject, string text)
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