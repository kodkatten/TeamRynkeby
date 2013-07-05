using System.ComponentModel.DataAnnotations;

namespace EventBooking.Controllers.ViewModels
{
    public class ForgottenPasswordViewModel
    {
        [Display(Name = "Epostadress")]
        public string Email { get; set; }

        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}