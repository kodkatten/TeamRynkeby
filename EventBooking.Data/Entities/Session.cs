using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Entities
{
    public interface ISession
    {
        int Id { get; set; }
        TimeSpan FromTime { get; set; }
        TimeSpan ToTime { get; set; }
        Activity Activity { get; set; }
        ICollection<User> Volunteers { get; set; }
        int VolunteersNeeded { get; set; }
        bool IsAllowedToSignUp(User user);
        bool CanLeave(User user);


    }
    public class Session : ISession
    {
        public int Id { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public Activity Activity { get; set; }
        public ICollection<User> Volunteers { get; set; }
        public int VolunteersNeeded { get; set; }

        public new bool IsAllowedToSignUp(User user)
        {
            return user != null &&
                   this.Volunteers.Count < this.VolunteersNeeded &&
                   this.Activity.OrganizingTeam.Id == user.Team.Id &&
                   this.Volunteers.All(volunteer => volunteer.Id != user.Id);
        }

        public bool CanLeave(User user)
        {
            return Volunteers.Any(v => v.Id == user.Id);
        }
    }
}