using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
	public class SessionViewModel
	{
		public SessionViewModel(Session sessionData, User user)
		{
			AvailablePlaces = sessionData.VolunteersNeeded - sessionData.Volunteers.Count();
			ToTimeFormatted = sessionData.ToTime.ToString();
			FromTimeFormatted = sessionData.FromTime.ToString();
			Id = sessionData.Id;
            CanSignUp = sessionData.IsAllowedToSignUp(user);
            CanLeave = sessionData.CanLeave(user);
		}

		public string FromTimeFormatted { get; private set; }

		public string ToTimeFormatted { get; private set; }

		public int AvailablePlaces { get; private set; }

        public bool CanSignUp { get; private set; }

        public bool CanLeave { get; private set; }

		public int Id { get; private set; }
	}
}