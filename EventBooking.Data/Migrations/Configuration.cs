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
                //context.Teams.Add(new Team {Name = "Team T�by"});
                //context.Teams.Add(new Team {Name = "Team Gr�nna/Jkp"});
                //context.Teams.Add(new Team {Name = "Team V�xj�"});
                //context.Teams.Add(new Team {Name = "Team Helsingborg"});
                //context.Teams.Add(new Team {Name = "Team Malm�"});
                context.SaveChanges();

            }
            if (!context.MailTemplates.Any())
            {
                var newActivityTemplate = new MailTemplate
                {
                    Name = "newactivity",
                    Subject = "Team Rynkeby - Ny aktivitet $ActivityName",
                    Content = @"Hej b�ste $Team medlem
Nu �r det en ny aktivitet p� g�ng.<br/>
$Date mellan $FirstTime och $LastTime<br/>

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
                context.SaveChanges();
            }
        }
    }
}
