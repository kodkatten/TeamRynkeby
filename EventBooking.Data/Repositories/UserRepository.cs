namespace EventBooking.Data.Repositories
{
    class UserRepository : IUserRepository
    {
        private readonly EventBookingContext context;

        public UserRepository(EventBookingContext context)
        {
            this.context = context;
        }

        public void Save(User user)
        {
            this.context.Users.Attach(user);
            this.context.SaveChanges();
        }
    }
}