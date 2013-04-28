using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class EditActivityViewModel
    {
        public ICollection<Session> Sessions { get; set; }
        public Activity Activity { get; set; }
    }
}