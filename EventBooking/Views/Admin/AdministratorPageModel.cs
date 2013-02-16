using System.Collections.Generic;
using EventBooking.Data;

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