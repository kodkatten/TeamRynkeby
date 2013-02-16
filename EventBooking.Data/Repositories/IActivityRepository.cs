using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventBooking.Data.Repositories
{
    public interface IActivityRepository
    {
        IQueryable<Activity> GetActivityByMonth(int year, int month, int teamId = 0);
    }
}
