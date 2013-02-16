using System.ComponentModel.DataAnnotations;
using System.Security;

namespace EventBooking.Controllers.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string ElectronicMailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
    }
}