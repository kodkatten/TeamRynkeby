using System.Web.Security;
using AutoMapper;
using Autofac;
using EventBooking.Data.Entities;
using EventBooking.Services;

namespace EventBooking.Controllers.ViewModels
{
	public class EventBookingMapper
	{
		public static void SetupMappers(IContainer container)
		{
			Mapper.CreateMap<CreateActivityModel, Activity>();
			Mapper.CreateMap<SessionModel, Session>();
			Mapper.CreateMap<Session, SessionModel>();
		    Mapper.CreateMap<MyProfileModel, User>();
		    Mapper.CreateMap<User, MyProfileModel>();
	
			var securityService = container.Resolve<ISecurityService>();

			Mapper.CreateMap<Team, TeamModel>()
				.ForMember(
					x => x.PowerUserRole,
					opt => opt.MapFrom(x => securityService.GetPowerUserRoleForTeam(x)));

			Mapper.CreateMap<User, UserModel>()
				.ForMember(
					x => x.Roles, 
					opt => opt.MapFrom(x => Roles.GetRolesForUser(x.Email)));
		}
	}
}