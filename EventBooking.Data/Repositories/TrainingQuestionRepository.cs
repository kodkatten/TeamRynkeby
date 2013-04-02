using System.Collections.Generic;
using System.Linq;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
    public class TrainingQuestionRepository : ITrainingQuestionRepository
    {
        private readonly IEventBookingContext _context;

        public TrainingQuestionRepository(IEventBookingContext context)
        {
            _context = context;
        }

        public IEnumerable<TrainingQuestion> GetAllByTeamId(int teamId)
        {
            var questions = _context.TrainingQuestions.Where(q => q.Team.Id == teamId);

            return questions;
        }

        public void Add(IEnumerable<TrainingQuestion> questions)
        {
            foreach (var trainingQuestion in questions)
            {
                _context.TrainingQuestions.Add(trainingQuestion);
            }

            _context.SaveChanges();
        }
    }
}
