namespace EventBooking.Services
{
    public interface IEmailService
    {
		void SendMail(int activityId, EmailService.EmailType emailType, string freeText = "");
    }
}
