using System.Collections.Generic;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
    public interface IInterviewQuestionRepository
    {
        IEnumerable<InterviewQuestion> GetAllByTeamId(int teamId);
        void Add(IEnumerable<InterviewQuestion> questions);
    }

    public interface ITrainingQuestionRepository
    {
        IEnumerable<TrainingQuestion> GetAllByTeamId(int teamId);
        void Add(IEnumerable<TrainingQuestion> questions);
    }
}