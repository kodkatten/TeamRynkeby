using System.ComponentModel.DataAnnotations;
using System.Security;

namespace EventBooking.Controllers.ViewModels
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Epostadress")]
        public string ElectronicMailAddress { get; set; }

        [Required]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
    }
}