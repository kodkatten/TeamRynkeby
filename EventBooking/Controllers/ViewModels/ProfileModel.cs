using System;
using System.ComponentModel.DataAnnotations;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class ProfileModel
    {
        [Required]
        [Display(Name="Namn")]
        public string Name { get; set; }

        [Display(Name="Gatuadress")]
        public string StreetAddress { get; set; }
        
        [Display(Name="Postnummer")]
        public string ZipCode { get; set; }
        
        [Display(Name = "Ort")]
        public string City { get; set; }
        
        [Required]
        [Display(Name = "Mobiltelefon")]
        public string Cellphone { get; set; }
        
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        
        [Required]
        [Display(Name = "Epostadress")]
        public string Epost { get; set; }
        
        [Display(Name = "Födelsedatum")]
        public DateTime Birthdate { get; set; }
        
        [Required]
        [Display(Name = "Team")]
        public Team Team { get; set; }
    }
}