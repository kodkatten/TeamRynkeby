using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
	public class MyProfileModel
	{
		[Display(Name = "Namn")]
		[Required(ErrorMessage = " * Namn måste anges")]
		[MinLength(2, ErrorMessage = "Ditt namn är för kort")]
		public string Name { get; set; }

		[Display(Name = "Gatuadress")]
		public string StreetAddress { get; set; }

		[Display(Name = "Postnummer")]
		[MinLength(5)]
		[MaxLength(6)]
		[RegularExpression(@"^(\d\d\d \d\d|\d\d\d\d\d)$", ErrorMessage = "Felaktigt postnummer")]
		public string ZipCode { get; set; }

		[Display(Name = "Ort")]
		public string City { get; set; }

		[Display(Name = "Mobiltelefon")]
		[Required(ErrorMessage = " * Mobiltelefon måste anges")]
		public string Cellphone { get; set; }

		[Display(Name = "Födelsedatum")]
        //[Range(typeof(DateTime), "1801-1-1", "2113-12-31", ErrorMessage = "{0} måste vara mellan {1:d} och {2:d}")]
        [Range(typeof(DateTime), "1801-1-1", "2113-12-31", ErrorMessage = "Det måste vara på formatet YYYY-MM-DD")]
		public DateTime? Birthdate { get; set; }

		[Display(Name = "Lag")]
		[Required(ErrorMessage = "Du måste välja ett lag")]
		public Team Team { get; set;}

		public IEnumerable<TeamModel> Teams { get; private set; }

		public MyProfileModel()
		{
		}

		public MyProfileModel(User currentUser, IEnumerable<Team> teams)
		{
			Mapper.Map(currentUser, this);
			this.Teams = teams.Select(AsTeamModel);
		}

		private static TeamModel AsTeamModel(Team team)
		{
			return Mapper.Map<TeamModel>(team);
		}

		public User ToUser()
		{
			return Mapper.Map<User>(this);
		}
	}
}