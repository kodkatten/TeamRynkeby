using System.ComponentModel.DataAnnotations;
using System.Security;

namespace EventBooking.Controllers.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = " * Måste anges")]
        [Display(Name = "Epostadress")]
        public string ElectronicMailAddress { get; set; }

        [Required(ErrorMessage = " * Måste anges")]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
    }
}