using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class MyProfileModel
    {
        [Display(Name="Namn")]
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Display(Name="Gatuadress")]
        public string StreetAddress { get; set; }
        
        [Display(Name="Postnummer")]
        [MinLength(5)]
        [MaxLength(6)]
        [RegularExpression(@"\d{3}\s{0,1}\d{2}")]
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

        public IEnumerable<TeamModel> Teams { get; private set; }

        public MyProfileModel()
        {
        }

        public MyProfileModel(IEnumerable<Team> teams)
        {
            this.Teams = teams.Select(AsTeamModel);
        }

        private static TeamModel AsTeamModel(Team team)
        {
            return new TeamModel(team);
        }
    }
}