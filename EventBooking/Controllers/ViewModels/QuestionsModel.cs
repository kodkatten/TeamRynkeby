using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventBooking.Controllers.ViewModels
{
    public class QuestionsModel
    {
        public IList<Question> InterviewQuestions { get; set; }
        public IList<Question> TrainingQuestions { get; set; }
    }
    
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
    }
}