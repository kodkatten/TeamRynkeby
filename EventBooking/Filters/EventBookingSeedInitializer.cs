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
            Database.SetInitializer(new CreateDatabaseIfNotExists<EventBookingContext>());
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "Id", "Email", autoCreateTables: true);

            var membership = (SimpleMembershipProvider) Membership.Provider;
            var roles = (SimpleRoleProvider) Roles.Provider;

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
            var team = new Team() {Name = "I R DA AWESOME TEAM"};

            context.Teams.Add(team);

            context.Activities.Add(new Activity
            {
                Name = "More awesome stuff.",
                Description = "Ham andouille spare ribs tongue pork loin tenderloin brisket. Sausage spare ribs pork loin cow flank ground round jerky beef ribs swine rump.",
                Date = new DateTime(2013, 02, 11),
                OrganizingTeam = team
            });

            context.Activities.Add(new Activity
            {
                Name = "Awesome aktivet uno",
                Description = "Bacon ipsum dolor sit amet boudin turducken fatback pancetta kielbasa pastrami doner cow capicola short ribs drumstick tail. ",
                Date = new DateTime(2013, 02, 03),
                OrganizingTeam = team
            });
        }
    }
}