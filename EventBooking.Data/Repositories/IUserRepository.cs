using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
	public interface IUserRepository
	{
		bool Exists(string email);
		void Save(User user);
		void RemoveFromTeam(int userId);
	}
}