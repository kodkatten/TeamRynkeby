using System.Collections.Generic;
using EventBooking.Data;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class AdministratorPageModel
    {
        public AdministratorPageModel(IEnumerable<Team> teams)
        {
            this.Teams = teams;
        }

        public IEnumerable<Team> Teams { get; private set; }


    }
}