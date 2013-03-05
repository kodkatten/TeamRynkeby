﻿using AutoMapper;
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
			Mapper.CreateMap<Session, SessionModel>();
		    Mapper.CreateMap<MyProfileModel, User>();
		    Mapper.CreateMap<User, MyProfileModel>();
			Mapper.CreateMap<Team, TeamModel>();
			Mapper.CreateMap<User, UserModel>();
		}
	}
}