using EventBooking.Data.Entities;

namespace EventBooking.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EventBookingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EventBooking.Data.EventBookingContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (!context.Teams.Any())
            {
                // TODO: Seed data here.
                context.Teams.Add(new Team { Name = "Team Stockholm" });
                //context.Teams.Add(new Team {Name = "Team Täby"});
                //context.Teams.Add(new Team {Name = "Team Gränna/Jkp"});
                //context.Teams.Add(new Team {Name = "Team Växjö"});
                //context.Teams.Add(new Team {Name = "Team Helsingborg"});
                //context.Teams.Add(new Team {Name = "Team Malmö"});
                context.SaveChanges();

            }
            if (!context.MailTemplates.Any())
            {
                var newActivityTemplate = new MailTemplate
                {
                    Name = "newactivity",
                    Subject = "Team Rynkeby - Ny aktivitet $ActivityName",
                    Content = @"Hej bäste $Team medlem
Nu är det en ny aktivitet på gång.<br/>
$Date mellan $FirstTime och $LastTime<br/>

$Summary


$Description"
                };

                context.MailTemplates.Add(newActivityTemplate);

                var activityInfoTemplate = new MailTemplate
                {
                    Name = "infoactivity",
                    Subject = "Team Rynkeby - Angående $ActivityName",
                    Content = @"<h4>Hej bäste ${Team}-medlem!</h4>

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

Vänligen<br/>
$ActivityManager"
                };
                context.MailTemplates.Add(activityInfoTemplate);
                context.SaveChanges();
            }
        }
    }
}
