using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
    public class UserMapper
    {

        public static void SetupMapper()
        {
            Mapper.CreateMap<User, User>().ForMember(u=>u.Id, opt=>opt.Ignore()).ForMember(u=>u.Email, opt=>opt.Ignore());
        }
    }
}