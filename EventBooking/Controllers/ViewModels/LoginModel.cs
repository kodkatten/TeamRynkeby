using System.ComponentModel.DataAnnotations;
using System.Security;

namespace EventBooking.Controllers.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Epostadress")]
        public string ElectronicMailAddress { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
    }
}