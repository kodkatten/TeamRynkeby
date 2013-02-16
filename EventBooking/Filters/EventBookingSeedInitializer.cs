using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using AutoMapper;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;

using WebMatrix.WebData;

namespace EventBooking.Filters
{
    internal class EventBookingSeedInitializer : DropCreateDatabaseAlways<EventBookingContext>
    {
        protected override void Seed(EventBookingContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "Id", "Email", autoCreateTables: true);
            UserMapper.SetupMapper();

            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;

            if (!roles.RoleExists(UserType.Administrator.ToString()))
                roles.CreateRole(UserType.Administrator.ToString());

            if (membership.GetUser("admin_test", false) == null)
                membership.CreateUserAndAccount("admin_test", "admin_test", new Dictionary<string, object> { { "Created", DateTime.Now } });

            if (!roles.GetRolesForUser("admin_test").Contains(UserType.Administrator.ToString()))
                roles.AddUsersToRoles(new[] { "admin_test" }, new[] { UserType.Administrator.ToString() });

            SeedActivitieshacketyHackBlaBla(context);
            CreateAwesomeUsers(membership, context);
        }

        private void CreateAwesomeUsers(SimpleMembershipProvider membership, EventBookingContext context)
        {
            EnsureUserExists(membership, context, "a@b.c");
            EnsureUserExists(membership,context, "email@email.com", new User { Cellphone = "3457", Name = "dodo", Team = context.Teams.First() });
            EnsureUserExists(membership,context, "a", new User { Cellphone = "3457", Name = "dodo2", Team = context.Teams.First() });
        }


        private static void EnsureUserExists(SimpleMembershipProvider membership, EventBookingContext context,
                                             string email, User specification = null)
        {
            if (membership.GetUser(email, false) != null) return;
            membership.CreateUserAndAccount(email, email,
                                            new Dictionary<string, object> {{"Created", DateTime.Now}});
            var user = context.Users.First(user1 => user1.Email == email);
            specification = specification ?? new User {Name = "One of the three very beared wise men"};
            Mapper.Map(specification, user);
            user.Created = DateTime.UtcNow;
            context.SaveChanges();
        }

        private static void SeedActivitieshacketyHackBlaBla(EventBookingContext context)
        {
            var team = new Team {Name = "I R DA AWESOME TEAM"};

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

            context.SaveChanges();
        }
    }
}