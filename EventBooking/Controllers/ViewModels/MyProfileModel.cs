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

       
        [Display(Name = "Mobiltelefon")]
        [Required(ErrorMessage = " * Mobiltelefon måste anges")]
        public string Cellphone { get; set; }

       

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