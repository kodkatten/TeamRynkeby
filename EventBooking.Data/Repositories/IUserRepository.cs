namespace EventBooking.Data.Repositories
{
	public interface IUserRepository
	{
		bool Exists(string email);
		void Save(User user);
	}
}