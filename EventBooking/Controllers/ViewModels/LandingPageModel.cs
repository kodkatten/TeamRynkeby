using System;
using System.Collections.Generic;

namespace EventBooking.Controllers.ViewModels
{
    public class  LandingPageModel
    {
        public LandingPageModel(IEnumerable<Data.Activity> enumerable)
        {
            this.Activities = new[]
                {
                    new ActivityModel
                        {
                            Description =
                                "Bacon ipsum dolor sit amet boudin turducken fatback pancetta kielbasa pastrami doner cow capicola short ribs drumstick tail. ",
                            DateFormatted = DateTime.Now.ToShortDateString(),
                            Name = "Awesome aktivet uno",
                            OrganizingTeam = "LULZ"
                        },
                    new ActivityModel
                        {
                            Description =
                                "Ham andouille spare ribs tongue pork loin tenderloin brisket. Sausage spare ribs pork loin cow flank ground round jerky beef ribs swine rump.",
                            DateFormatted = DateTime.Now.ToShortDateString(),
                            Name = "More awesome stuff.",
                            OrganizingTeam = "CATZ"
                        }
                };
        }

        public IEnumerable<ActivityModel> Activities { get; internal set; }
    }
}