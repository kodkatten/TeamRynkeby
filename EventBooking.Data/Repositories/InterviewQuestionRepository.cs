using System.Collections.Generic;
using System.Linq;

namespace EventBooking.Data.Repositories
{
    public class InterviewQuestionRepository : IInterviewQuestionRepository
    {
        private readonly IEventBookingContext context;

        public InterviewQuestionRepository(IEventBookingContext context)
        {
            this.context = context;
        }

        public IEnumerable<InterviewQuestion> GetAllByTeamId(int teamId)
        {
            var questions = context.InterviewQuestions.Where(q => q.Team.Id == teamId);

            return questions;
        }

        public virtual void Add(IEnumerable<InterviewQuestion> questions)
        {
            foreach (var interviewQuestion in questions)
            {
                context.InterviewQuestions.Add(interviewQuestion);
            }

            context.SaveChanges();
        }

    }
}
