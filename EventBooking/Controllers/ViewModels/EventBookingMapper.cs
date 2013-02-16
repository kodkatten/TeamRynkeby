using AutoMapper;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class EventBookingMapper
	{
		public static void SetupMappers()
		{
			Mapper.CreateMap<CreateActivityModel, Activity>();
		    Mapper.CreateMap<MyProfileModel, User>();
		    Mapper.CreateMap<User, MyProfileModel>();
		}
	}
}