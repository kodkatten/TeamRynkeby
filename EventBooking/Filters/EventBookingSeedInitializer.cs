using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;

using WebMatrix.WebData;

namespace EventBooking.Filters
{
    internal class EventBookingSeedInitializer : DropCreateDatabaseAlways<EventBookingContext>   // DropCreateDatabaseIfModelChanges<EventBookingContext> //
	{
		protected override void Seed(EventBookingContext context)
		{
			WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "Id", "Email", autoCreateTables: true);

			var membership = (SimpleMembershipProvider)Membership.Provider;
			var roles = (SimpleRoleProvider)Roles.Provider;

			EnsureRole(roles, UserType.Administrator.ToString());
			EnsureRole(roles, UserType.PowerUser.ToString());

			SeedActivities(context);

			var firstTeam = context.Teams.First();
			if (membership.GetUser("henrik.andersson@tretton37.com", false) == null)
				EnsureUserExists(membership, context, "henrik.andersson@tretton37.com",
					new User
						{
							Cellphone = "13457",
							Name = "dodo",
							Team = firstTeam,
							Email = "henrik.andersson@tretton37.com"
						});

			if (!roles.GetRolesForUser("henrik.andersson@tretton37.com").Contains(UserType.Administrator.ToString()))
				roles.AddUsersToRoles(new[] { "henrik.andersson@tretton37.com" }, new[] { UserType.Administrator.ToString() });

			if (membership.GetUser("tidaholm69@hotmail.com", false) == null)
			{
				var user = new User
				{
					Cellphone = "13457",
					Name = "najz",
					Email = "henrik.andersson@tretton37.com",
					Team = firstTeam,  
				};			
											
				EnsureUserExists(membership, context, "tidaholm69@hotmail.com", user);
			}


			var teamPowerUser = firstTeam.Name + " PowerUser";
			EnsureRole(roles, teamPowerUser);
			if (!roles.GetRolesForUser("tidaholm69@hotmail.com").Contains(teamPowerUser))
				roles.AddUsersToRoles(new[] { "tidaholm69@hotmail.com" }, new[] { teamPowerUser });

			CreateAwesomeUsers(membership, context);
			CreatePredefinedActivityItems(context);
			var session = context.Activities.First();
			session.Coordinator = context.Users.First();
			context.SaveChanges();
		}

		private static void EnsureRole(SimpleRoleProvider roles, string role)
		{
			if (!roles.RoleExists(role))
				roles.CreateRole(role);
		}

		private void CreateAwesomeUsers(SimpleMembershipProvider membership, EventBookingContext context)
		{
			EnsureUserExists(membership, context, "henrik.andersson@tretton37.com", new User { Cellphone = "123455", Name = "henrik", Team = context.Teams.First(), Email = "henrik.andersson@tretton37.com" });
			EnsureUserExists(membership, context, "tidaholm69@hotmail.com", new User { Cellphone = "3457", Name = "Henkepolarn", Team = context.Teams.First(), Email = "tidaholm69@hotmail.com" });
			EnsureUserExists(membership, context, "henkepolarn@gmail.com", new User { Cellphone = "3457", Name = "Henkepolarn", Team = context.Teams.First(), Email = "henkepolarn@gmail.com" });
			EnsureUserExists(membership, context, "henkeofsweden@live.com", new User { Cellphone = "3457", Name = "Henkeofsweden", Team = context.Teams.First(), Email = "henkeofsweden@live.com" });
			EnsureUserExists(membership, context, "jhofstam@hotmail.com", new User { Cellphone = "3457", Name = "Johanna", Team = context.Teams.First(), Email = "jhofstam@hotmail.com" });
		}

		private void CreatePredefinedActivityItems(EventBookingContext context)
		{
			context.ActivityItemTemplates.Add(new ActivityItemTemplate() { Name = "Cykel" });
			context.ActivityItemTemplates.Add(new ActivityItemTemplate() { Name = "Trainer" });
			context.ActivityItemTemplates.Add(new ActivityItemTemplate() { Name = "T�lt" });
			context.ActivityItemTemplates.Add(new ActivityItemTemplate() { Name = "Bord" });
			context.ActivityItemTemplates.Add(new ActivityItemTemplate() { Name = "Priser" });
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

		private static void SeedActivities(EventBookingContext context)
		{
			var team = new Team { Name = "Team T�by" };
			context.Teams.Add(team);
		    var dateTime = DateTime.Now.AddDays(2);

            //var activity = new Activity
            //    {
            //        Name = "Insamling i T�by Centrum",
            //        Description = "Under sportlovet kommer Team Rynkeby vara i T�by Centrum",
            //        Date = dateTime,
            //        OrganizingTeam = team,
            //        Type = ActivityType.Prelimin�rt
            //    };
            //var session = new Session
            //{WhoHasSignup.cshtml 
            //    FromTime = new TimeSpan(8, 0, 0),
            //    ToTime = new TimeSpan(10, 0, 0),
            //    Activity = activity,
            //    Volunteers = new Collection<User>(),
            //    VolunteersNeeded = 15
            //};
            //context.Activities.Add(activity);
            //context.Sessions.Add(session);


		    var activity1 = new Activity
		        {
                    Name = "Stor insamling",
		            Summary = "Stor insamling",
                    Description = "Stor insamling",
		            Date = dateTime.AddDays(1),
		            OrganizingTeam = team,
		            Type = ActivityType.Publikt
		        };

            var session1 = new Session
            {
                FromTime = new TimeSpan(8, 0, 0),
                ToTime = new TimeSpan(10, 0, 0),
                Activity = activity1,
                Volunteers = new Collection<User>(),
                VolunteersNeeded = 15
            };
            context.Activities.Add(activity1);
		    context.Sessions.Add(session1);





            //// Team #2
            //var team2 = new Team() { Name = "Team Stockholm" };
            //team2.Activities = new List<Activity>();
            //team2.Activities.Add(new Activity()
            //{
            //    Name = "Tanter p� stan",
            //    Description = "Dessa skall allts� r�nas.",
            //    Summary = "En g�ng var jag tv� g�nger.",
            //    Date = new DateTime(2013, 02, 22),
            //    OrganizingTeam = team2
            //});

            //team2.Activities.Add(new Activity()
            //{
            //    Name = "Samla in pengar p� stan",
            //    Summary = "Stora h�gtidsdagen d� alla vill sk�nka pengar",
            //    Description = "Det �r inte alla som har pengar, men de kan alltid sk�nka en lite slant, och m�nga b�ckar sm�, eller m�nga slantar sm�, leder till en rikedom f�r mig",
            //    Date = new DateTime(2013, 03, 03),
            //    OrganizingTeam = team2
            //});
            //team2.Activities.Add(new Activity()
            //{
            //    Name = "En annan aktivitet",
            //    Summary = "Stora tiggardagen",
            //    Description = "Vi sk�nker v�ra sj�lar till satan.",
            //    Date = new DateTime(2013, 03, 03),
            //    OrganizingTeam = team2
            //});

            //team2.Activities.Add(new Activity()
            //{
            //    Name = "adfa",
            //    Summary = "adfasdfa",
            //    Description = "adfafafdsdfasdfasdf.",
            //    Date = new DateTime(2013, 02, 28),
            //    OrganizingTeam = team2
            //});
            //context.Teams.Add(team2);

			context.SaveChanges();
		}
	}
}