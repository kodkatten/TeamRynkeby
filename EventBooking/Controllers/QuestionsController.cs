using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Repositories;

namespace EventBooking.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IInterviewQuestionRepository interviewQuestionRepository;
        private readonly ITrainingQuestionRepository trainingQuestionRepository;

        public QuestionsController(IInterviewQuestionRepository interviewQuestionRepository,
            ITrainingQuestionRepository trainingQuestionRepository)
        {
            this.interviewQuestionRepository = interviewQuestionRepository;
            this.trainingQuestionRepository = trainingQuestionRepository;
        }

        //
        // GET: /Questions/

        public ActionResult Create(int teamId)
        {
            var model = new QuestionsModel {TeamId = teamId};
            model.NumberOfQuestionRows = 15;

            model.InterviewQuestions = Enumerable.Range(0, model.NumberOfQuestionRows).Select(i => String.Empty).ToList();
            model.TrainingQuestions = Enumerable.Range(0, model.NumberOfQuestionRows).Select(i => String.Empty).ToList();

            return View(model);
        }


        //
        // POST: /Questions/Create

        [HttpPost]
        public ActionResult Create(QuestionsModel model)
        {

            interviewQuestionRepository.Add(model.InterviewQuestions.Select(q =>
                new InterviewQuestion
                    {
                        QuestionText = q,
                        TeamId = model.TeamId
                    }));

            trainingQuestionRepository.Add(model.TrainingQuestions.Select(q =>
                new TrainingQuestion
                    {
                        QuestionText = q,
                        TeamId = model.TeamId
                    }));


            return View(model);

        }


    }
}
