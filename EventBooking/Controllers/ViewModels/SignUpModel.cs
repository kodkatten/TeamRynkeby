using System.ComponentModel.DataAnnotations;

namespace EventBooking.Controllers.ViewModels
{
    public class SignUpModel
    {
        [Display(Name="Epostadress")]
        [Required(ErrorMessage = " * Måste anges")]
        [EmailAddress(ErrorMessage = "Du måste ange en korrekt epostadress")]
        public string Email { get; set; }

        [Display(Name="Lösenord")]
        [Required(ErrorMessage = " * Måste anges")]
        [MinLength(6, ErrorMessage = "6 tecken eller fler")]
        public string Password { get; set; }

        [Display(Name="Bekräfta lösenord")]
        [Required(ErrorMessage = " * Måste anges")]
        [Compare("Password", ErrorMessage = "Lösenorden stämmer inte med varandra.")]
        public string ConfirmPassword { get; set; }

        //[Display(Name="Kom ihåg mig")]
        //[Required]
        //public bool RememberMe { get; set; }
    }
}