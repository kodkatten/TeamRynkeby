using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EventBooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventBooking.Controllers.ViewModels
{
    public class QuestionsModel
    {
        public QuestionsModel()
        {
            InterviewQuestions = new List<string>();
            TrainingQuestions = new List<string>();
        }
        public IList<string> InterviewQuestions { get; set; }
        public IList<string> TrainingQuestions { get; set; }
        
        [Required]
        [HiddenInput]
        public int TeamId { get; set; }

        public int NumberOfQuestionRows { get; set; }
    }

}