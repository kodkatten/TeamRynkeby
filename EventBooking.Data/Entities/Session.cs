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

    //class SessionComparer : IEqualityComparer<ISession>
    //{
    //    public bool Equals(ISession x, ISession y)
    //    {
    //        //Check whether the compared objects reference the same data. 
    //        if (Object.ReferenceEquals(x, y)) return true;

    //        //Check whether any of the compared objects is null. 
    //        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
    //            return false;

    //        //Check whether the products' properties are equal. 
    //        return x.ToTime == y.ToTime && x.FromTime == y.FromTime && x.VolunteersNeeded == y.VolunteersNeeded;
    //    }

    //    public int GetHashCode(ISession obj)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


}