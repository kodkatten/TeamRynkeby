using System.Linq;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class SessionViewModel
	{
		public SessionViewModel(Session sessionData, User user)
		{
			this.AvailablePlaces = sessionData.VolunteersNeeded - sessionData.Volunteers.Count();
			this.ToTimeFormatted = sessionData.ToTime.ToString();
			this.FromTimeFormatted = sessionData.FromTime.ToString();
			this.Id = sessionData.Id;
			this.CanSignUp = sessionData.IsAllowedToSignUp(user);
		}

		public string FromTimeFormatted { get; private set; }

		public string ToTimeFormatted { get; private set; }

		public int AvailablePlaces { get; private set; }

		public bool CanSignUp { get; private set; }

		public int Id { get; private set; }
	}
}