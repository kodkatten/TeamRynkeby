using System.Linq;

namespace EventBooking.Data.Repositories
{
	public interface IActivityRepository
	{
		IQueryable<Activity> GetActivityByMonth(int year, int month, int teamId = 0);
	}
}