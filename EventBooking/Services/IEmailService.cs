using System.Linq;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data.Entities;

namespace EventBooking.Services
{
    public interface IEmailService
    {
		void SendMail(int activityId, EmailService.EmailType emailType, string freeText = "");
        void SendReminderMail(int activityId, IQueryable<User> senderList, EmailService.EmailType emailType, string freeText = "");
		MailData GetPreview(int activityId, EmailService.EmailType emailType, string freeText);
        void SendResetPassword(string email, string message);
    }
}
