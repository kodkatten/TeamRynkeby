using EventBooking.Data.Repositories;
using EventBooking.Services;

namespace EventBooking.Tests
{
    class MockupEmailService :EmailService 
    {
        public MockupEmailService(ActivityRepository activityRepository, ITeamRepository teamRepository) 
            : base(activityRepository, teamRepository)
        {
        }
    }
}
