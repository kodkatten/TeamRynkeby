﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EventBooking.Controllers.ViewModels
{
    public class EditSessionModel
    {
        public int SessionId { get; set; }
        public int ActivityId { get; set; }

        [Display(Name = "Till tid")]
        public TimeSpan ToTime { get; set; }
        
        [Display(Name = "Från tid")]
        public TimeSpan FromTime { get; set; }
        
        [Display(Name = "Antalet deltagare")]
        public int VolunteersNeeded { get; set; }

    }
}