namespace EventBooking.Data.Repositories
{
    public interface IUserRepository
    {
        void Save(User user);
    }

    class UserRepository : IUserRepository
    {
        public void Save(User user)
        {
        }
    }
}