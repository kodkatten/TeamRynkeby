using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Queries
{
    public class GetActivitiesByMonthQuery
    {
        private readonly int month;

        public delegate GetActivitiesByMonthQuery Factory(int month);

        public GetActivitiesByMonthQuery(EventBookingContext context, int month)
        {
            this.month = month;
        }

        public IEnumerable<Activity> Execute()
        {
            return new[]
                {
                    new Activity
                        {
                            Name = "Awesome aktivet uno",
                            Description = "Bacon ipsum dolor sit amet boudin turducken fatback pancetta kielbasa pastrami doner cow capicola short ribs drumstick tail. ",
                            Date = new DateTime(2013, 02, 03),
                            OrganizingTeam = new Team { Name = "I R TEAM"}
                        },
                    new Activity
                        {
                            Name = "More awesome stuff.",
                            Description = "Ham andouille spare ribs tongue pork loin tenderloin brisket. Sausage spare ribs pork loin cow flank ground round jerky beef ribs swine rump.",
                            Date = new DateTime(2013, 02, 11),
                            OrganizingTeam = new Team { Name = "U IZ TEAM!"}
                        }
                };
        }
    }
}