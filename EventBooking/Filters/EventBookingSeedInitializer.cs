using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Security;
using WebMatrix.WebData;

namespace EventBooking.Filters
{
    internal class EventBookingSeedInitializer : DropCreateDatabaseIfModelChanges<EventBookingContext> //  DropCreateDatabaseAlways<EventBookingContext> // 
    {
        protected override void Seed(EventBookingContext context)
        {
            
           // WebSecurity.InitializeDatabaseConnection("EventBookingContext", "Users", "Id", "Email", autoCreateTables: true);

            WebSecurityInitializer.Instance.EnsureInitialize();
            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;

            EnsureRole(roles, UserType.Administrator.ToString());
            EnsureRole(roles, UserType.PowerUser.ToString());

            CreateDefaultMailTemplates(context);


            CreateTeams(context);
            var teamStockholm = context.Teams.First();
            if (membership.GetUser("henrik.andersson@tretton37.com", false) == null)
            {
                EnsureUserExists(membership, context, "henrik.andersson@tretton37.com",
                    new User
                        {
                            Cellphone = "0727-133740",
                            Name = "Henrik Andersson",
                            Team = teamStockholm,
                            Email = "henrik.andersson@tretton37.com"
                        });
            }

            if (membership.GetUser("henkeofsweden@live.com", false) == null)
            {
                EnsureUserExists(membership, context, "henkeofsweden@live.com",
                    new User
                    {
                        Cellphone = "0727-133740",
                        Name = "Henrik Andersson",
                        Team = teamStockholm,
                        Email = "henkeofsweden@live.com"
                    });
            }
            if (!roles.GetRolesForUser("henrik.andersson@tretton37.com").Contains(UserType.Administrator.ToString()))
            {
                roles.AddUsersToRoles(new[] { "henrik.andersson@tretton37.com" }, new[] { UserType.Administrator.ToString() });
            }

            if (!roles.GetRolesForUser("henkeofsweden@live.com").Contains(UserType.Administrator.ToString()))
            {
                roles.AddUsersToRoles(new[] { "henkeofsweden@live.com" }, new[] { UserType.PowerUser.ToString() });
            }

            var activities = context.Activities.ToList();
            activities.ForEach(x => x.Coordinator = context.Users.First());

            context.SaveChanges();
        }

        private void CreateTeams(EventBookingContext context)
        {
            context.Teams.Add(new Team { Name = "Team Stockholm" });
            context.SaveChanges();
        }

        private static void CreateDefaultMailTemplates(EventBookingContext context)
        {
            var newActivityTemplate = new MailTemplate
                {
                    Name = "newactivity",
                    Subject = "Team Rynkeby - Ny aktivitet $ActivityName",
                    Content = @"Hej b�ste $Team medlem
Nu �r det en ny aktivitet p� g�ng.
$Date mellan $FirstTime och $LastTime

$Summary


$Description"
                };
            context.MailTemplates.Add(newActivityTemplate);

            var activityInfoTemplate = new MailTemplate
                {
                    Name = "infoactivity",
                    Subject = "Team Rynkeby - Ang�ende $ActivityName",
                    Content = @"<h4>Hej b�ste ${Team}-medlem!</h4>

<h5>$ActivityName - $Date mellan $FirstTime och $LastTime</h5>
<p>$Summary</p>

<p>$FreeText</p>

<table>
#foreach ( $User in $Users )
	<tr><td style='border-bottom: solid 1px #000;'><b>$User.Name</b></td><td style='border-bottom: solid 1px #000;'>$User.CellPhone</td></tr>
#foreach ( $Session in $User.Sessions )
	<tr><td colspan='2' style='padding-left: 25px;'>$Session.FromTime - $Session.ToTime</td></tr>
#end
#end
</table>

<p>$Description</p>

V�nligen<br/>
$ActivityManager"
                };
            context.MailTemplates.Add(activityInfoTemplate);
        }

        private static void EnsureRole(SimpleRoleProvider roles, string role)
        {
            if (!roles.RoleExists(role))
                roles.CreateRole(role);
        }

        private void CreateAwesomeUsers(SimpleMembershipProvider membership, EventBookingContext context)
        {
            EnsureUserExists(membership, context, "henrik.andersson@tretton37.com", new User { Cellphone = "123455", Name = "henrik", Team = context.Teams.First(), Email = "henrik.andersson@tretton37.com" });
        }

        private static void EnsureUserExists(SimpleMembershipProvider membership, EventBookingContext context,
                                             string email, User specification = null)
        {
            if (membership.GetUser(email, false) != null)
            {
                return;
            }

            var result = membership.CreateUserAndAccount(email, email, new Dictionary<string, object> { { "Created", DateTime.Now } });
            var user = context.Users.First(user1 => user1.Email == email);
            specification = specification ?? new User { Name = "One of the three very beared wise men" };
            UserMapper.MapUserTemp(user, specification);
            user.Created = DateTime.UtcNow;
            //context.Sessions.First().Volunteers.Add(user);
            context.SaveChanges();
        }
    }
}