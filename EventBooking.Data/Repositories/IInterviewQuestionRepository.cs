using System.Collections.Generic;

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