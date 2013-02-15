using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;

using EventBooking.Data;

using WebMatrix.WebData;

namespace EventBooking.Filters
{
    internal class EventBookingSeedInitializer : /*IDatabaseInitializer<EventBookingContext>*/ DropCreateDatabaseAlways<EventBookingContext>
    {
        protected override void Seed(EventBookingContext context)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<EventBookingContext>());
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "Id", "Email", autoCreateTables: true);

            var seeder = new FluentMembership((SimpleMembershipProvider) Membership.Provider, (SimpleRoleProvider) Roles.Provider);
            seeder.CreateUser("admin_test").WithRole(UserType.Administrator);
            seeder.CreateRole(UserType.Default).ForUser("default_test");

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

        internal class FluentMembership
        {
            private readonly SimpleMembershipProvider membership;
            private readonly SimpleRoleProvider roles;

            public FluentMembership(SimpleMembershipProvider membership, SimpleRoleProvider roles)
            {
                this.membership = membership;
                this.roles = roles;
            }

            public FluentUser CreateUser(string username)
            {
                return new FluentUser(this, username).CreateUser();
            }

            public FluentRole CreateRole(UserType role)
            {
                return new FluentRole(this, role).CreateRole();
            }

            internal class FluentRole
            {
                private readonly FluentMembership fluentMembership;
                private readonly UserType role;

                public FluentRole(FluentMembership fluentMembership, UserType role)
                {
                    this.fluentMembership = fluentMembership;
                    this.role = role;
                }

                public FluentRole CreateRole()
                {
                    if (!fluentMembership.roles.RoleExists(role.ToString()))
                        fluentMembership.roles.CreateRole(role.ToString());

                    return this;
                }

                public void ForUser(string username)
                {
                    new FluentUser(fluentMembership, username)
                        .CreateUser();

                    if (!fluentMembership.roles.GetRolesForUser(username).Contains(role.ToString()))
                        fluentMembership.roles.AddUsersToRoles(new[] { username }, new[] { role.ToString() });
                }
            }

            internal class FluentUser
            {
                private readonly FluentMembership fluentMembership;
                private readonly string username;

                public FluentUser(FluentMembership fluentMembership, string username)
                {
                    this.fluentMembership = fluentMembership;
                    this.username = username;
                }

                public FluentUser CreateUser()
                {
                    if (fluentMembership.membership.GetUser(username, false) == null)
                        fluentMembership.membership.CreateUserAndAccount(username, username, new Dictionary<string, object>{{"Created", DateTime.Now}});

                    return this;
                }

                public FluentUser WithRole(UserType role)
                {
                    new FluentRole(fluentMembership, role).CreateRole().ForUser(this.username);

                    return this;
                }
            }
        }

        public void InitializeDatabase(EventBookingContext context)
        {
            Seed(context);
        }
    }
}