using EventBooking.Controllers.ViewModels;

namespace EventBooking.Services
{
    public interface IEmailService
    {
		void SendMail(int activityId, EmailService.EmailType emailType, string freeText = "");
		MailData GetPreview(int activityId, EmailService.EmailType emailType, string freeText);
    }
}
