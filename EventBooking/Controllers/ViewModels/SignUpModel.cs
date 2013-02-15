using System.ComponentModel.DataAnnotations;

namespace EventBooking.Controllers.ViewModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Du måste ange en korrekt epostadress")]
        [Display(Name="Epostadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [MinLength(6, ErrorMessage = "6 tecken eller fler")]
        [Display(Name="Lösenord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Lösenorden stämmer inte med varandra.")]
        [Display(Name="Bekräfta lösenord")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name="Kom ihåg mi")]
        public bool RememberMe { get; set; }
    }
}