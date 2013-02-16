using AutoMapper;
using Autofac;
using EventBooking.Data;
using EventBooking.Services;

namespace EventBooking.Controllers.ViewModels
{
	public class EventBookingMapper
	{
		public static void SetupMappers(IContainer container)
		{
			Mapper.CreateMap<CreateActivityModel, Activity>();
			Mapper.CreateMap<SessionModel, Session>();
		    Mapper.CreateMap<MyProfileModel, User>();
		    Mapper.CreateMap<User, MyProfileModel>();

			var securityService = container.Resolve<ISecurityService>();

			Mapper.CreateMap<User, UserModel>()
				.ForMember(x => x.IsTeamAdmin, x => securityService.IsUserTeamAdmin());
		    
			Mapper.CreateMap<Team, TeamModel>();
		}
	}
}