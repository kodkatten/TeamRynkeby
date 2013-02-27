using System.ComponentModel.DataAnnotations;
using EventBooking.Controllers.ViewModels;

namespace EventBooking.Validators
{
    public class ExistingSession : ValidationAttribute
    {
        public ExistingSession()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validator = new CustomValidator();
            var newSession = (SessionModel) value;
            var currentSessions = (ActivitySessionsModel)validationContext.ObjectInstance;
              
            if (validator.IsSessionIntrudingOnOtherSessionsTimeframe(newSession,currentSessions.Sessions))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}