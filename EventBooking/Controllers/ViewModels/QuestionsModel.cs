using EventBooking.Data;
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
        public int TeamId { get; set; }
    }
    
    public class Question
    {
        public string QuestionText { get; set; }
    }
}