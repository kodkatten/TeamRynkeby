using System.ComponentModel.DataAnnotations;

namespace EventBooking.Controllers.ViewModels
{
    public class SignUpModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "Lösenorden stämmer inte med varandra.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}