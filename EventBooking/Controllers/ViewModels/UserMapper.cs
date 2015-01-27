using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
    public class UserMapper
    {
        public static User MapUserTemp(User destination, User source)
        {
            destination.Name = source.Name;
            destination.Cellphone = source.Cellphone;
            destination.Team = source.Team;
            destination.Created = source.Created;

            return destination;
        }
    }
}