using System;
using System.ComponentModel.DataAnnotations;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class MyProfileModel
    {
        [Display(Name="Namn")]
        [Required]
        public string Name { get; set; }

        [Display(Name="Gatuadress")]
        public string StreetAddress { get; set; }
        
        [Display(Name="Postnummer")]
        public string ZipCode { get; set; }
        
        [Display(Name = "Ort")]
        public string City { get; set; }
        
        [Display(Name = "Mobiltelefon")]
        [Required]
        public string Cellphone { get; set; }
        
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        
        [Display(Name = "Födelsedatum")]
        public DateTime Birthdate { get; set; }
        
        [Display(Name = "Team")]
        [Required]
        public Team Team { get; set; }
    }
}