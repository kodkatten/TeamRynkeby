using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            
            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;

            if (!roles.RoleExists(UserType.Administrator.ToString()))
                roles.CreateRole(UserType.Administrator.ToString());

            if (!roles.RoleExists(UserType.PowerUser.ToString()))
                roles.CreateRole(UserType.PowerUser.ToString());

			SeedActivities( context );

            if (membership.GetUser("henrik.andersson@tretton37.com", false) == null)
				EnsureUserExists( membership, context, "Henrik.andersson@tretton37.com", 
                    new User
                        {
                            Cellphone = "3457",
                            Name = "dodo",
                            Team = context.Teams.First(),
                            Email = "Henrik.andersson@tretton37.com"
                        } );

            if (!roles.GetRolesForUser("henrik.andersson@tretton37.com").Contains(UserType.Administrator.ToString()))
                roles.AddUsersToRoles(new[] { "henrik.andersson@tretton37.com" }, new[] { UserType.Administrator.ToString() });

            if (membership.GetUser("tidaholm69@hotmail.com", false) == null)
                EnsureUserExists(membership, context, "tidaholm69@hotmail.com", new User { Cellphone = "3457", Name = "najz", Email = "Henrik.andersson@tretton37.com", Team = context.Teams.First() });

            if (!roles.GetRolesForUser("tidaholm69@hotmail.com").Contains(UserType.PowerUser.ToString()))
                roles.AddUsersToRoles(new[] { "tidaholm69@hotmail.com" }, new[] { UserType.PowerUser.ToString() });

            CreateAwesomeUsers(membership, context);
	        CreatePredefinedActivityItems(context);
            var session = context.Activities.First();
            session.Coordinator = context.Users.First();
            context.SaveChanges();
        }

        private void CreateAwesomeUsers(SimpleMembershipProvider membership, EventBookingContext context)
        {
            EnsureUserExists(membership, context, "henrik.andersson@tretton37.com", new User { Cellphone = "123455", Name = "henrik", Team = context.Teams.First(), Email = "henrik.andersson@tretton37.com" });
            EnsureUserExists(membership, context, "tidaholm69@hotmail.com",new User { Cellphone = "3457", Name = "Henkepolarn", Team = context.Teams.First(), Email = "tidaholm69@hotmail.com"});
            EnsureUserExists(membership,context, "henkepolarn@gmail.com", new User { Cellphone = "3457", Name = "Henkepolarn", Team = context.Teams.First(), Email = "henkepolarn@gmail.com"});
            EnsureUserExists(membership,context, "henkeofsweden@live.com", new User { Cellphone = "3457", Name = "Henkeofsweden", Team = context.Teams.First(),Email = "henkeofsweden@live.com"});
            EnsureUserExists(membership, context, "jhofstam@hotmail.com", new User { Cellphone = "3457", Name = "Johanna", Team = context.Teams.First(), Email = "jhofstam@hotmail.com" });
        }

		private void CreatePredefinedActivityItems(EventBookingContext context)
		{
			context.PredefinedActivityItems.Add(new PredefinedActivityItem() { Name = "Cykel" });
			context.PredefinedActivityItems.Add(new PredefinedActivityItem() { Name = "Trainer" });
			context.PredefinedActivityItems.Add(new PredefinedActivityItem() { Name = "Tält" });
			context.PredefinedActivityItems.Add(new PredefinedActivityItem() { Name = "Bord" });
			context.PredefinedActivityItems.Add(new PredefinedActivityItem() { Name = "Priser" });
		}

        private static void EnsureUserExists(SimpleMembershipProvider membership, EventBookingContext context,
                                             string email, User specification = null)
        {
            if (membership.GetUser(email, false) != null)
            {
                return;
            }

            var result = membership.CreateUserAndAccount(email, email, new Dictionary<string, object> {{"Created", DateTime.Now}});
            var user = context.Users.First(user1 => user1.Email == email);
            specification = specification ?? new User {Name = "One of the three very beared wise men"};
            UserMapper.MapUserTemp(user, specification);          
            user.Created = DateTime.UtcNow;
            context.Sessions.First().Volunteers.Add(user);
            context.SaveChanges();
        }

        private static void SeedActivities(EventBookingContext context)
        {
            var team = new Team {Name = "Team Täby"};
            context.Teams.Add(team);
            var dateTime = new DateTime(2013, 02, 17);

            var activity = new Activity
                {
                    Name = "Insamling i Täby Centrum",
                    Description = "Under sportlovet kommer Team Rynkeby vara i Täby Centrum", 
                    Date = dateTime, 
                    OrganizingTeam = team, 
                    Type = ActivityType.Preliminärt
                };
            var session = new Session {
                FromTime = new TimeSpan(8,0,0),
                ToTime = new TimeSpan(10,0,0),
                Activity = activity,
                Volunteers = new Collection<User>(),
                VolunteersNeeded = 15
            };
            context.Activities.Add(activity);
            context.Sessions.Add(session);
            context.Activities.Add(new Activity
            {
                Name = "Insamling i Täby Centrum",
                Summary = "Stor insamling",
                Description = "Under sportlovet kommer Team Rynkeby vara i Täby Centrum",
                Date = new DateTime(2013, 02, 27),
                OrganizingTeam = team,
                Type = ActivityType.Publikt
            }); 
            context.Activities.Add(new Activity
            {
                Name = "Insamling vid Pendeltågsstationen",
                Description = "Skit i 3 koppar latte. Skänk dem till ",
                Date = new DateTime(2013, 03, 17),
                OrganizingTeam = team,
                Type = ActivityType.Sponsor
            });
            context.Activities.Add(new Activity
            {
                Name = "Spin of Hope",
                Description = "Cykla av dig fläsket ",
                Date = new DateTime(2013, 03, 27),
                OrganizingTeam = team,
                Type = ActivityType.Träning
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

            context.SaveChanges();
        }
    }
}