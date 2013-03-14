using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBooking.Controllers.ViewModels;
using EventBooking.Data;
using EventBooking.Data.Entities;
using EventBooking.Data.Repositories;

namespace EventBooking.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IInterviewQuestionRepository interviewQuestionRepository;
        private readonly ITrainingQuestionRepository trainingQuestionRepository;
        private readonly IUserRepository userRepository;
        private readonly ITeamRepository teamRepository;

        public QuestionsController(IInterviewQuestionRepository interviewQuestionRepository,
            ITrainingQuestionRepository trainingQuestionRepository, IUserRepository userRepository, ITeamRepository teamRepository)
        {
            this.interviewQuestionRepository = interviewQuestionRepository;
            this.trainingQuestionRepository = trainingQuestionRepository;
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;
        }

        //
        // GET: /Questions/

        public ActionResult Create(int teamId)
        {
            var team = teamRepository.Get(teamId);

            var model = new QuestionsModel
                {
                    TeamId = teamId,
                    NumberOfQuestionRows = 15,
                    Volunteers = team.Volunteers.Select(v => new Volunteer {Id = v.Id, Name = v.Name}).ToList()
                };

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

        public ActionResult DeleteVolunteers(IList<Volunteer> volunteers, int teamId, FormCollection form)
        {
            foreach (var volunteer in volunteers.Where(v => v.IsSelectedForRemoval))
            {
                userRepository.RemoveFromTeam(volunteer.Id);
            }

            return RedirectToAction("Create", new {teamId });
        }


    }
}
