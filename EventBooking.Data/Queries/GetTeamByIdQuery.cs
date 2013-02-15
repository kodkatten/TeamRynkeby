using System;
using EventBooking.Data.Extensions;

namespace EventBooking.Data.Queries
{
    public class GetTeamByIdQuery
    {
        private readonly int _id;

        public GetTeamByIdQuery(int id)
        {
            _id = id;
        }

        public Team Execute()
        {
            return new Team
                {


                    Activities = new[]
                        {
                            new Activity
                                {
                                    Name = "Fake activity",
                                    Description = "A description",
                                    Date = DateTime.Now,
                                    Summary = "A summary",
                                    Coordinator = new User {Name = "Tomten"},
                                    RequiredItems = new Item[] {},
                                    Sessions = new Session[0],
                                    Type = ActivityType.Public
                                },
                            new Activity
                                {
                                    Name = "Fake activity2",
                                    Description = "A description",
                                    Date = 30.Days().FromNow(),
                                    Summary = "A summary",
                                    Coordinator = new User {Name = "Tomten"},
                                    RequiredItems = new Item[] {},
                                    Sessions = new Session[0],
                                    Type = ActivityType.Public
                                }
                        },
                    Name = "Malmö"
                };
        }
    }
}