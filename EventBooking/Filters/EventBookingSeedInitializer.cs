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

			SeedActivitieshacketyHackBlaBla( context );
			
			if ( membership.GetUser( "admin_test", false ) == null )
				EnsureUserExists( membership, context, "admin_test", new User { Cellphone = "3457", Name = "dodo", Team = context.Teams.First() } );

            if (!roles.GetRolesForUser("admin_test").Contains(UserType.Administrator.ToString()))
                roles.AddUsersToRoles(new[] { "admin_test" }, new[] { UserType.Administrator.ToString() });
            
            if (membership.GetUser("poweruser", false) == null)
                EnsureUserExists(membership, context, "poweruser", new User { Cellphone = "3457", Name = "najz", Team = context.Teams.First() });

            if (!roles.GetRolesForUser("poweruser").Contains(UserType.PowerUser.ToString()))
                roles.AddUsersToRoles(new[] { "poweruser" }, new[] { UserType.PowerUser.ToString() });

            CreateAwesomeUsers(membership, context);
	        CreatePredefinedActivityItems(context);
            var session = context.Activities.First();
            session.Coordinator = context.Users.First();
            context.SaveChanges();
        }

        private void CreateAwesomeUsers(SimpleMembershipProvider membership, EventBookingContext context)
        {
            EnsureUserExists(membership, context, "a@b.c");
            EnsureUserExists(membership,context, "email@email.com", new User { Cellphone = "3457", Name = "dodo", Team = context.Teams.First() });
            EnsureUserExists(membership,context, "a", new User { Cellphone = "3457", Name = "dodo2", Team = context.Teams.First() });
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

        private static void SeedActivitieshacketyHackBlaBla(EventBookingContext context)
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
                    Type = ActivityType.Preliminary,
                    
                };
            var session = new Session {
                FromTime = dateTime.AddHours(8),
                ToTime = dateTime.AddHours(10),
                Activity = activity,
                Volunteers = new Collection<User>()
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
                Type = ActivityType.Public
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
                Type = ActivityType.Training
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