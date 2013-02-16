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
                Name = "Tanter på stan",
                Description = "Dessa skall alltså rånas.",
                Summary = "En gång var jag två gånger.",
                Date = new DateTime(2013, 02, 22),
                OrganizingTeam = team2
            });

            team2.Activities.Add(new Activity()
            {
                Name = "Samla in pengar på stan",
                Summary = "Stora högtidsdagen då alla vill skänka pengar",
                Description = "Det är inte alla som har pengar, men de kan alltid skänka en lite slant, och många bäckar små, eller många slantar små, leder till en rikedom för mig",
                Date = new DateTime(2013, 03, 03),
                OrganizingTeam = team2
            });
            team2.Activities.Add(new Activity()
            {
                Name = "En annan aktivitet",
                Summary = "Stora tiggardagen",
                Description = "Vi skänker våra själar till satan.",
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