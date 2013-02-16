using AutoMapper;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class EventBookingMapper
	{
		public static void SetupMappers()
		{
			Mapper.CreateMap<CreateActivityModel, Activity>();
			Mapper.CreateMap<SessionModel, Session>();
		    Mapper.CreateMap<MyProfileModel, User>();
		    Mapper.CreateMap<User, MyProfileModel>();
		}
	}
}