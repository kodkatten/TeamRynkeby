using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;

using EventBooking.Data;

using WebMatrix.WebData;

namespace EventBooking.Filters
{
    internal class EventBookingSeedInitializer : DropCreateDatabaseAlways<EventBookingContext>
    {
        protected override void Seed(EventBookingContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "Id", "Email", autoCreateTables: true);

            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;

            if (!roles.RoleExists(UserType.Administrator.ToString()))
                roles.CreateRole(UserType.Administrator.ToString());

            if (membership.GetUser("admin_test", false) == null)
                membership.CreateUserAndAccount("admin_test", "admin_test", new Dictionary<string, object> { { "Created", DateTime.Now } });

            if (!roles.GetRolesForUser("admin_test").Contains(UserType.Administrator.ToString()))
                roles.AddUsersToRoles(new[] { "admin_test" }, new[] { UserType.Administrator.ToString() });

            SeedActivitieshacketyHackBlaBla(context);
        }

        private static void SeedActivitieshacketyHackBlaBla(EventBookingContext context)
        {
            // Team #1
            var team = new Team() {Name = "I R DA AWESOME TEAM"};
            context.Teams.Add(team);
            context.Activities.Add(new Activity
            {
                Name = "More awesome stuff.",
                Description = "Ham andouille spare ribs tongue pork loin tenderloin brisket. Sausage spare ribs pork loin cow flank ground round jerky beef ribs swine rump.",
                Date = new DateTime(2013, 02, 17),
                OrganizingTeam = team
            });
            context.Activities.Add(new Activity
            {
                Name = "Awesome aktivet uno",
                Description = "Bacon ipsum dolor sit amet boudin turducken fatback pancetta kielbasa pastrami doner cow capicola short ribs drumstick tail. ",
                Date = new DateTime(2013, 02, 27),
                OrganizingTeam = team
            });

            // Team #2
            var team2 = new Team() {Name = "Team Stockholm"};
            team2.Activities = new List<Activity>();
            team2.Activities.Add(new Activity()
            {
                Name = "Tanter p� stan",
                Description = "Dessa skall allts� r�nas.",
                Summary = "En g�ng var jag tv� g�nger.",
                Date = new DateTime(2013, 02, 22),
                OrganizingTeam = team2
            });

            team2.Activities.Add(new Activity()
            {
                Name = "Samla in pengar p� stan",
                Summary = "Stora h�gtidsdagen d� alla vill sk�nka pengar",
                Description = "Det �r inte alla som har pengar, men de kan alltid sk�nka en lite slant, och m�nga b�ckar sm�, eller m�nga slantar sm�, leder till en rikedom f�r mig",
                Date = new DateTime(2013, 03, 03),
                OrganizingTeam = team2
            });
            team2.Activities.Add(new Activity()
            {
                Name = "En annan aktivitet",
                Summary = "Stora tiggardagen",
                Description = "Vi sk�nker v�ra sj�lar till satan.",
                Date = new DateTime(2013, 03, 03),
                OrganizingTeam = team2
            });

            team2.Activities.Add(new Activity()
            {
                Name = "adfa",
                Summary = "adfasdfa",
                Description = "adfafafdsdfasdfasdf.",
                Date = new DateTime(2013, 02, 28),
                OrganizingTeam = team2
            });
            context.Teams.Add(team2);
        }
    }
}