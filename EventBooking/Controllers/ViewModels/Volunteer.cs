using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventBooking.Controllers.ViewModels
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelectedForRemoval { get; set; }
    }
}
