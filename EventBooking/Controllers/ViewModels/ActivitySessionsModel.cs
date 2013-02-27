using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventBooking.Validators;

namespace EventBooking.Controllers.ViewModels
{
    public class ActivitySessionsModel
    {
        private readonly ActivityModel _activity;
        private readonly List<SessionModel> _sessions;

        public ActivitySessionsModel(ActivityModel activity, IEnumerable<SessionModel> sessions)
        {
            _activity = activity;
            _sessions = new List<SessionModel>(sessions);
            SelectedSession = new SessionModel(_activity.Id);
        }

        public ActivitySessionsModel()
        {
            _sessions = new List<SessionModel>();
            _activity = new ActivityModel();
        }

        public ActivityModel Activity { get { return _activity; } }
        [Required]
        [ExistingSession(ErrorMessage = "Pass inom det intervallet existerar redan")]
        public SessionModel SelectedSession { get; set; }
        public List<SessionModel> Sessions { get { return _sessions; } }
    }
}